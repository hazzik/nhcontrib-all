using System;
using System.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Mapping;
using NHibernate.Properties;
using NHibernate.Util;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;
using Environment=NHibernate.Validator.Engine.Environment;

namespace NHibernate.Validator.Event
{
	/// <summary>
	/// Before insert and update, executes the validator framework
	/// </summary>
	public class ValidateEventListener : IPreInsertEventListener, IPreUpdateEventListener, IInitializable
	{
		private bool isInitialized;
		private Dictionary<System.Type, ValidatableElement> validators = new Dictionary<System.Type, ValidatableElement>();

		#region IInitializable Members

		/// <summary>
		/// Initialize the validators, any non significant validators are not kept
		/// </summary>
		/// <param name="cfg"></param>
		public void Initialize(Configuration cfg)
		{
			if (isInitialized) return;

			IMessageInterpolator interpolator = GetInterpolator(cfg);

			ICollection<PersistentClass> classes = cfg.ClassMappings;

			foreach(PersistentClass clazz in classes)
			{
				System.Type mappedClass = clazz.MappedClass;
				ClassValidator validator = GetClassValidator(mappedClass, interpolator);

				ValidatableElement element = new ValidatableElement(mappedClass, validator);
				AddSubElement(clazz.IdentifierProperty, element);

				foreach(Property property in clazz.PropertyIterator)
				{
					AddSubElement(property, element);
				}

				if (element.HasSubElements || element.Validator.HasValidationRules)
					validators.Add(mappedClass, element);
			}

			isInitialized = true;
		}

		#endregion

		protected virtual ClassValidator GetClassValidator(System.Type mappedClass, IMessageInterpolator interpolator)
		{
			return
				new ClassValidator(mappedClass, null, null, interpolator, new Dictionary<System.Type, ClassValidator>(),
				                   ValidatorMode.UseAttribute);
		}

		#region IPreInsertEventListener Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="event"></param>
		/// <returns></returns>
		public bool OnPreInsert(PreInsertEvent @event)
		{
			Validate(@event.Entity, @event.Source.EntityMode);
			return false;
		}

		#endregion

		#region IPreUpdateEventListener Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="event"></param>
		/// <returns></returns>
		public bool OnPreUpdate(PreUpdateEvent @event)
		{
			Validate(@event.Entity, @event.Source.EntityMode);
			return false;
		}

		#endregion

		/// <summary>
		/// Get the custom <see cref="IMessageInterpolator"/> from the <see cref="Configuration"/>
		/// </summary>
		/// <param name="cfg"></param>
		/// <returns></returns>
		private IMessageInterpolator GetInterpolator(Configuration cfg)
		{
			string interpolatorString = cfg.GetProperty(Environment.MessageInterpolatorClass);
			IMessageInterpolator interpolator = null;

			if (!string.IsNullOrEmpty(interpolatorString))
			{
				try
				{
					System.Type interpolatorType = ReflectHelper.ClassForName(interpolatorString);
					interpolator = (IMessageInterpolator) Activator.CreateInstance(interpolatorType);
				}
				catch(MissingMethodException ex)
				{
					throw new HibernateException("Public constructor was not found at message interpolator: " + interpolatorString, ex);
				}
				catch(InvalidCastException ex)
				{
					throw new HibernateException(
						"Type does not implement the interface " + typeof(IMessageInterpolator).GetType().Name + ": " + interpolatorString,
						ex);
				}
				catch(Exception ex)
				{
					throw new HibernateException("Unable to instanciate message interpolator: " + interpolatorString, ex);
				}
			}
			return interpolator;
		}

		/// <summary>
		/// Add sub elements. Composite Elements.
		/// </summary>
		/// <param name="property"></param>
		/// <param name="element"></param>
		private void AddSubElement(Property property, ValidatableElement element)
		{
			if (property != null && property.IsComposite && !property.BackRef)
			{
				Component component = (Component) property.Value;
				if (component.IsEmbedded) return;

				IPropertyAccessor accesor = PropertyAccessorFactory.GetPropertyAccessor(property, EntityMode.Poco);

				IGetter getter = accesor.GetGetter(element.EntityType, property.Name);

				ClassValidator validator = new ClassValidator(getter.ReturnType);

				ValidatableElement subElement = new ValidatableElement(getter.ReturnType, validator, getter);

				foreach(Property currentProperty in component.PropertyIterator)
				{
					AddSubElement(currentProperty, subElement);
				}

				if (subElement.HasSubElements || subElement.Validator.HasValidationRules)
					element.AddSubElement(subElement);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="mode"></param>
		protected void Validate(object entity, EntityMode mode)
		{
			if (entity == null || !EntityMode.Poco.Equals(mode)) return;

			ValidatableElement element;

			if (isInitialized)
				if (validators.ContainsKey(entity.GetType()))
					element = validators[entity.GetType()];
				else
					return; //no validation to do
			else
				throw new AssertionFailure("Validator event not initialized");

			List<InvalidValue> consolidatedInvalidValues = new List<InvalidValue>();
			ValidateSubElements(element, entity, consolidatedInvalidValues);
			InvalidValue[] invalidValues = element.Validator == null ? null : element.Validator.GetInvalidValues(entity);
			if (invalidValues != null)
			{
				foreach(InvalidValue invalidValue in invalidValues)
				{
					consolidatedInvalidValues.Add(invalidValue);
				}
			}
			if (consolidatedInvalidValues.Count > 0)
				throw new InvalidStateException(consolidatedInvalidValues.ToArray(), entity.GetType().Name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <param name="entity"></param>
		/// <param name="consolidatedInvalidValues"></param>
		private void ValidateSubElements(ValidatableElement element, Object entity,
		                                 IList<InvalidValue> consolidatedInvalidValues)
		{
			if (element != null)
			{
				foreach(ValidatableElement subElement in element.SubElements)
				{
					Object component = subElement.Getter.Get(entity);

					InvalidValue[] invalidValues = subElement.Validator.GetInvalidValues(component);

					foreach(InvalidValue invalidValue in invalidValues)
					{
						consolidatedInvalidValues.Add(invalidValue);
					}

					ValidateSubElements(subElement, component, consolidatedInvalidValues);
				}
			}
		}
	}
}
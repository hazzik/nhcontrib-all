using System;
using System.Collections.Generic;
using System.Persistence;
using log4net;
using NHibernate.Annotations.Extensions;
using NHibernate.Annotations.NHibernate.Cfg;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Util;

namespace NHibernate.Annotations.Cfg.Annotations
{
	public class EntityBinder
	{
		private System.Type annotatedClass;
		private int batchSize;
		private string cacheConcurrentStrategy;
		private bool cacheLazyProperty;
		private string cacheRegion;
		private string discriminatorValue = string.Empty;
		private bool dynamicInsert;
		private bool dynamicUpdate;
		private bool explicitHibernateEntityAnnotation;
		private IDictionary<String, String> filters = new Dictionary<String, String>();
		private bool ignoreIdAnnotations;
		private InheritanceState inheritanceState;
		private bool isPropertyAnnotated = false;
		private bool lazy;
		private ILog log = LogManager.GetLogger(typeof (EntityBinder));
		private ExtendedMappings mappings;
		private String name;
		private OptimisticLockType optimisticLockType;
		private PersistentClass persistentClass;
		private PolymorphismType polymorphismType;
		private String propertyAccessor;
		private System.Type proxyClass;
		private IDictionary<String, Object> secondaryTableJoins = new Dictionary<String, Object>();
		private IDictionary<String, Join> secondaryTables = new Dictionary<String, Join>();
		private bool selectBeforeUpdate;
		private String where;

		public bool IsAnnotatedProperty
		{
			get { return isPropertyAnnotated; }
		}

		/// <summary>
		/// Use as a fake one for Collection of elements
		/// </summary>
		public EntityBinder()
		{
		}

		public EntityBinder(
			EntityAttribute hibAnn,
			System.Type annotatedClass,
			PersistentClass persistentClass,
			ExtendedMappings mappings
			)
		{
			this.mappings = mappings;
			this.persistentClass = persistentClass;
			this.annotatedClass = annotatedClass;
			BindHibernateAnnotation(hibAnn);
		}

		private void BindHibernateAnnotation(EntityAttribute hibAnn)
		{
			if (hibAnn != null)
			{
				dynamicInsert = hibAnn.IsDynamicInsert;
				dynamicUpdate = hibAnn.IsDynamicUpdate;
				optimisticLockType = hibAnn.OptimisticLock;
				selectBeforeUpdate = hibAnn.IsSelectBeforeUpdate;
				polymorphismType = hibAnn.Polymorphism;
				explicitHibernateEntityAnnotation = true;
				//persister handled in bind
			}
			else
			{
				//default values when the annotation is not there
				dynamicInsert = false;
				dynamicUpdate = false;
				optimisticLockType = OptimisticLockType.Version;
				polymorphismType = PolymorphismType.Implicit;
				selectBeforeUpdate = false;
			}
		}

		public void SetDiscriminatorValue(String discriminatorValue)
		{
			this.discriminatorValue = discriminatorValue;
		}

		public void BindEntity()
		{
			persistentClass.IsAbstract = annotatedClass.IsAbstract;
			persistentClass.ClassName = annotatedClass.Name;
			persistentClass.NodeName = name;
			//TODO:review this
			//persistentClass.setDynamic(false); //no longer needed with the Entity name refactoring?
			persistentClass.EntityName = (annotatedClass.Name);
			BindDiscriminatorValue();

			persistentClass.IsLazy = lazy;
			if (proxyClass != null)
			{
				persistentClass.ProxyInterfaceName = proxyClass.Name;
			}
			persistentClass.DynamicInsert = dynamicInsert;
			persistentClass.DynamicUpdate = dynamicUpdate;

			if (persistentClass is RootClass)
			{
				RootClass rootClass = (RootClass) persistentClass;
				bool mutable = true;
				//priority on @Immutable, then @Entity.mutable()
				if (annotatedClass.IsAttributePresent<ImmutableAttribute>())
				{
					mutable = false;
				}
				else
				{
					var entityAnn = annotatedClass.GetAttribute<EntityAttribute>();
					if (entityAnn != null)
					{
						mutable = entityAnn.IsMutable;
					}
				}
				rootClass.IsMutable = mutable;
				rootClass.IsExplicitPolymorphism = ExplicitPolymorphismConverter.Convert(polymorphismType);
				if (StringHelper.IsNotEmpty(where)) rootClass.Where = where;

				if (cacheConcurrentStrategy != null)
				{
					rootClass.CacheConcurrencyStrategy = cacheConcurrentStrategy;
					rootClass.CacheRegionName = cacheRegion;
					//TODO: LazyPropertiesCacheable
					//rootClass.LazyPropertiesCacheable =  cacheLazyProperty ;
				}
				rootClass.IsForceDiscriminator = annotatedClass.IsAttributePresent<ForceDiscriminatorAttribute>();
			}
			else
			{
				if (explicitHibernateEntityAnnotation)
				{
					log.WarnFormat("[NHibernate.Annotations.Entity] used on a non root entity: ignored for {0}", annotatedClass.Name);
				}
				if (annotatedClass.IsAttributePresent<ImmutableAttribute>())
				{
					log.WarnFormat("[Immutable] used on a non root entity: ignored for {0}", annotatedClass.Name);
				}
			}
			persistentClass.OptimisticLockMode = OptimisticLockModeConverter.Convert(optimisticLockType);
			persistentClass.SelectBeforeUpdate = selectBeforeUpdate;

			//set persister if needed
			//[Persister] has precedence over [Entity.persister]
			var persisterAnn = annotatedClass.GetAttribute<PersisterAttribute>();
			System.Type persister = null;
			if (persisterAnn != null)
			{
				persister = persisterAnn.Implementation;
			}
			else
			{
				var entityAnn = annotatedClass.GetAttribute<EntityAttribute>();
				if (entityAnn != null && !BinderHelper.IsDefault(entityAnn.Persister))
				{
					try
					{
						persister = ReflectHelper.ClassForName(entityAnn.Persister);
					}
					catch (TypeLoadException tle)
					{
						throw new AnnotationException("Could not find persister class: " + persister, tle);
					}
				}
			}
			if (persister != null) persistentClass.EntityPersisterClass = persister;
			persistentClass.BatchSize = batchSize;

			//SQL overriding
			var sqlInsert = annotatedClass.GetAttribute<SQLInsertAttribute>();
			var sqlUpdate = annotatedClass.GetAttribute<SQLUpdateAttribute>();
			var sqlDelete = annotatedClass.GetAttribute<SQLDeleteAttribute>();
			var sqlDeleteAll = annotatedClass.GetAttribute<SQLDeleteAllAttribute>();
			var loader = annotatedClass.GetAttribute<LoaderAttribute>();

			if (sqlInsert != null)
			{
				persistentClass.SetCustomSQLInsert(sqlInsert.Sql.Trim(), sqlInsert.Callable,
				                                   ExecuteUpdateResultCheckStyleConverter.Convert(sqlInsert.Check));
			}
			if (sqlUpdate != null)
			{
				persistentClass.SetCustomSQLUpdate(sqlUpdate.Sql.Trim(), sqlUpdate.Callable,
				                                   ExecuteUpdateResultCheckStyleConverter.Convert(sqlUpdate.Check));
			}
			if (sqlDelete != null)
			{
				persistentClass.SetCustomSQLDelete(sqlDelete.Sql, sqlDelete.Callable,
				                                   ExecuteUpdateResultCheckStyleConverter.Convert(sqlDelete.Check));
			}
			if (sqlDeleteAll != null)
			{
				persistentClass.SetCustomSQLDelete(sqlDeleteAll.Sql, sqlDeleteAll.Callable,
				                                   ExecuteUpdateResultCheckStyleConverter.Convert(sqlDeleteAll.Check));
			}
			if (loader != null)
			{
				persistentClass.LoaderName = loader.NamedQuery;
			}

			//tuplizers
			if (annotatedClass.IsAttributePresent<TuplizerAttribute>())
			{
				foreach (TuplizerAttribute tuplizer in annotatedClass.GetAttributes<TuplizerAttribute>())
				{
					var mode = EntityModeConverter.Convert(tuplizer.EntityMode);
					persistentClass.AddTuplizer(mode, tuplizer.Implementation.Name);
				}
			}
			if (annotatedClass.IsAttributePresent<TuplizerAttribute>())
			{
				var tuplizer = annotatedClass.GetAttribute<TuplizerAttribute>();
				var mode = EntityModeConverter.Convert(tuplizer.EntityMode);
				persistentClass.AddTuplizer(mode, tuplizer.Implementation.Name);
			}

			if (!inheritanceState.HasParents)
			{
				var iter = filters.GetEnumerator();
				while (iter.MoveNext())
				{
					var filter = iter.Current;
					String filterName = filter.Key;
					String cond = filter.Value;
					if (BinderHelper.IsDefault(cond))
					{
						FilterDefinition definition = mappings.GetFilterDefinition(filterName);
						cond = definition == null ? null : definition.DefaultFilterCondition;
						if (StringHelper.IsEmpty(cond))
							throw new AnnotationException("no filter condition found for filter " + filterName + " in " + name);
					}
					persistentClass.AddFilter(filterName, cond);
				}
			}
			else
			{
				if (filters.Count > 0)
				{
					log.WarnFormat("@Filter not allowed on subclasses (ignored): {0}", persistentClass.EntityName);
				}
			}
			log.DebugFormat("Import with entity name {0}", name);

			try
			{
				mappings.AddImport(persistentClass.EntityName, name);
				String entityName = persistentClass.EntityName;
				if (!entityName.Equals(name))
				{
					mappings.AddImport(entityName, entityName);
				}
			}
			catch (MappingException me)
			{
				throw new AnnotationException("Use of the same entity name twice: " + name, me);
			}
		}

		private bool IsExplicitPolymorphism(PolymorphismType type)
		{
			throw new NotImplementedException();
		}

		private void BindDiscriminatorValue()
		{
			if (StringHelper.IsEmpty(discriminatorValue))
			{
				IValue discriminator = persistentClass.Discriminator;
				if (discriminator == null)
				{
					persistentClass.DiscriminatorValue = name;
				}
				else if ("character".Equals(discriminator.GetType().Name))
				{
					throw new AnnotationException("Using default @DiscriminatorValue for a discriminator of type CHAR is not safe");
				}
				else if ("integer".Equals(discriminator.GetType().Name))
				{
					//TODO: review if it's correct
					//persistentClass.DiscriminatorValue = String.valueOf(name.GetHashCode());
					persistentClass.DiscriminatorValue = name.GetHashCode().ToString();
				}
				else
				{
					persistentClass.DiscriminatorValue = name; //Spec compliant
				}
			}
			else
			{
				//persistentClass.getDiscriminator()
				persistentClass.DiscriminatorValue = discriminatorValue;
			}
		}

		public Join AddJoin(JoinTableAttribute ann, ClassPropertyHolder propertyHolder, bool creation)
		{
			throw new NotImplementedException();
		}

		public IDictionary<string, Join> SecondaryTables
		{
			get { return secondaryTables; }
		}

		public void FinalSecondaryTableBinding(PropertyHolder holder)
		{
			throw new NotImplementedException();
		}
	}
}
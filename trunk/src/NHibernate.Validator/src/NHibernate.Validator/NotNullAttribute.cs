using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// Ensure the member to be not null.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(NotNullValidator))]
	public class NotNullAttribute : Attribute, IRuleArgs
	{
		private string message = "{validator.notEmpty}";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}
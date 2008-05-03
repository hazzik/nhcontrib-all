using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (CreditCardNumberValidator))]
	public class CreditCardNumberAttribute : Attribute, IRuleArgs
	{
		private string message = "{validator.creditCardNumber}";

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion
	}
}
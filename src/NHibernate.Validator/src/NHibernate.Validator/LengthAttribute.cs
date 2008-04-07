namespace NHibernate.Validator
{
	using System;

	/// <summary>
	/// Apply some length restrictions to the annotated element. It has to be a string
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(LengthValidator))]
	public class LengthAttribute : Attribute, IHasMessage
	{
		private int min = 0;
		private int max = int.MaxValue;
		private string message = "{validator.length}";

		public LengthAttribute(int min, int max)
		{
			this.max = max;
			this.min = min;
		}

		public LengthAttribute(int max)
		{
			this.max = max;
		}

		public LengthAttribute()
		{
		}

		public int Min
		{
			get { return min; }
			set { min = value; }
		}

		public int Max
		{
			get { return max; }
			set { max = value; }
		}

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}
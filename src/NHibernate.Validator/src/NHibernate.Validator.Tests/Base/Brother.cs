using System;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Base
{
	public class Brother
	{
		[NotNull, Valid] private Address address;

		private Brother elder;
		private String name;
		private Brother youngerBrother;

		public Address Address
		{
			get { return address; }
			set { address = value; }
		}

		[NotNull]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		[Valid]
		public Brother Elder
		{
			get { return elder; }
			set { elder = value; }
		}

		[Valid]
		public Brother YoungerBrother
		{
			get { return youngerBrother; }
			set { youngerBrother = value; }
		}

		public override bool Equals(object obj)
		{
			return true;
		}

		public override int GetHashCode()
		{
			return 5;
		}
	}

	public class BrotherDef: ValidationDef<Brother>
	{
		public BrotherDef()
		{
			Define(x => x.Address).NotNullable().And.IsValid();
			Define(x => x.Name).NotNullable();
			Define(x => x.Elder).IsValid();
			Define(x => x.YoungerBrother).IsValid();
		}
	}
}
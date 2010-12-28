using Iesi.Collections.Generic;

namespace NHibernate.Envers.Tests.Integration.Inheritance.Joined.NotOwnedRelation
{
	[Audited]
	public class Contact
	{
		public virtual long Id { get; set; }
		public virtual string Email { get; set; }
		public virtual ISet<Address> Addresses { get; set; }
	}
}
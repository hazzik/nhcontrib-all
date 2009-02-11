using System;
using System.Linq.Expressions;
using NHibernate.Validator.Mappings;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IValidationDefinition<T> where T : class
	{
		IDateTimeConstraints Define(Expression<Func<T, DateTime>> property);
		IDateTimeConstraints Define(Expression<Func<T, DateTime?>> property);
		
		IBooleanConstraints Define(Expression<Func<T, bool>> property);
		IBooleanConstraints Define(Expression<Func<T, bool?>> property);

		IClassMapping GetMapping();
	}
}
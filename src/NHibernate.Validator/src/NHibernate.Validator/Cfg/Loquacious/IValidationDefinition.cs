using System;
using System.Collections;
using System.Linq.Expressions;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IValidationDefinition<T> where T : class
	{
		IDateTimeConstraints Define(Expression<Func<T, DateTime>> property);
		IDateTimeConstraints Define(Expression<Func<T, DateTime?>> property);
		
		IBooleanConstraints Define(Expression<Func<T, bool>> property);
		IBooleanConstraints Define(Expression<Func<T, bool?>> property);

		IStringConstraints Define(Expression<Func<T, string>> property);

		ICollectionConstraints Define(Expression<Func<T, ICollection>> property);
	}
}
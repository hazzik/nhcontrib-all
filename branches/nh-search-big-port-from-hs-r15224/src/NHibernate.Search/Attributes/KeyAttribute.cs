using System;
using NHibernate.Search.Bridge;

namespace NHibernate.Search.Attributes
{
    /// <summary>
    /// Marks a method as a key constructor for a given type.
    /// A key is an object that uniquely identify a given object type and a given set of parameters
    ///
    /// The key object must implement equals / hashcode so that 2 keys are equals iif
    /// the given target object types are the same, the set of parameters are the same.
    ///
    /// <see cref="FactoryAttribute"/> currently works for FullTextFilterDef.impl classes
    /// </summary>
    [AttributeUsage(AttributeTargets.Method,  AllowMultiple = false)]
    public class KeyAttribute : Attribute
    {
    }
}
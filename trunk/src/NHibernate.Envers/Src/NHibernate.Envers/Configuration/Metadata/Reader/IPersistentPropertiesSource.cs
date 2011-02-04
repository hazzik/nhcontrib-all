﻿using System.Collections.Generic;
using NHibernate.Mapping;

namespace NHibernate.Envers.Configuration.Metadata.Reader
{
    public interface IPersistentPropertiesSource
    {
        IEnumerable<Property> PropertyEnumerator { get; }
    	System.Type Clazz { get; }
    	Property VersionedProperty { get; }
    }
}
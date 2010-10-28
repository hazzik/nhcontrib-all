﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Iesi.Collections.Generic;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Entities.Mapper.Relation.Query;
using NHibernate.Envers.Exceptions;
using NHibernate.Envers.Reader;

namespace NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor
{
    public class SetCollectionInitializor<T> : AbstractCollectionInitializor<ISet<T>>
	{
        private readonly System.Type collectionType;
        private readonly MiddleComponentData elementComponentData;

    	public SetCollectionInitializor(AuditConfiguration verCfg,
											IAuditReaderImplementor versionsReader,
											IRelationQueryGenerator queryGenerator,
											Object primaryKey, long revision,
											System.Type collectionType,
											MiddleComponentData elementComponentData) 
								:base(verCfg, versionsReader, queryGenerator, primaryKey, revision)
        {
            this.collectionType = collectionType;
            this.elementComponentData = elementComponentData;
        }

        protected override ISet<T> InitializeCollection(int size) 
		{
            try
            {
                return (ISet<T>) Activator.CreateInstance(collectionType);
            } 
			catch (InstantiationException e) 
			{
                throw new AuditException(e);
            } 
			catch (SecurityException e) 
			{
                throw new AuditException(e);
            }
        }

        protected override void AddToCollection(ISet<T> collection, object collectionRow) 
		{
            var elementData = ((IList) collectionRow)[elementComponentData.ComponentIndex];

            // If the target entity is not audited, the elements may be the entities already, so we have to check
            // if they are maps or not.
            T element;
            if (elementData is IDictionary) 
			{
                element = (T)elementComponentData.ComponentMapper.MapToObjectFromFullMap(entityInstantiator,
                        (IDictionary<String, Object>) elementData, null, revision);
            } 
			else 
			{
                element = (T)elementData;
            }
			collection.Add(element);
        }
    }
}

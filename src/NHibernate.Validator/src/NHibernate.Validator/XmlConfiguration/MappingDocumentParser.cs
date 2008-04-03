using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using NHibernate.Validator.MappingSchema;
using System.IO;

namespace NHibernate.Validator
{
    public class MappingDocumentParser : IMappingDocumentParser
    {
        private readonly XmlSerializer serializer = new XmlSerializer(typeof(NhvValidator));

        public NhvValidator Parse(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            return (NhvValidator)serializer.Deserialize(stream);
        }
	}
}

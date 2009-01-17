using System;
using Iesi.Collections.Generic;
using NHibernate.Search.Attributes;

namespace NHibernate.Search.Tests.Query
{
    [Indexed(Index = "Book")]
    public class Book
    {
        private int id;
        private String summary;
        private String body;
        private ISet<Author> authors = new HashedSet<Author>();
        private Author mainAuthor;
        private DateTime publicationDate;

        public Book()
        {
        }

        public Book(int id, string summary, string body)
        {
            this.id = id;
            this.body = body;
            this.summary = summary;
            this.publicationDate = DateTime.Now;
        }

        [DocumentId]
        public virtual int Id
        {
            get { return id; }
            set { id = value; }
        }

        [Field(Index.Tokenized, Store = Attributes.Store.No)]
        public virtual string Body
        {
            get { return body; }
            set { body = value; }
        }

        [Field(Index.Tokenized, Store = Attributes.Store.Yes)]
        [Field(Index.UnTokenized, Name = "summary_forSort")]
        public virtual string Summary
        {
            get { return summary; }
            set { summary = value; }
        }

        [IndexedEmbedded]
        public virtual Author MainAuthor
        {
            get { return mainAuthor; }
            set { mainAuthor = value; }
        }
        
        public virtual ISet<Author> Authors
        {
            get { return authors; }
            set { authors = value; }
        }

        [Field(Index.UnTokenized, Store = Attributes.Store.Yes)]
        [DateBridge(Resolution.Second)]
        public virtual DateTime PublicationDate
        {
            get { return publicationDate; }
            set { publicationDate = value; }
        }
    }
}
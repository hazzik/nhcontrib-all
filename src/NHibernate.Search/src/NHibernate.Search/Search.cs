using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using NHibernate.Criterion;
using NHibernate.Search.Impl;

namespace NHibernate.Search {
    public static class Search {
        public static IFullTextSession CreateFullTextSession(ISession session) {
            return new FullTextSessionImpl(session);
        }

        public static ICriterion Query(Lucene.Net.Search.Query luecneQuery) {
            return new LuceneQueryExpression(luecneQuery);
        }

        public static ICriterion Query(string luceneQuery) {
            QueryParser parser = new QueryParser("", new StandardAnalyzer());
            Lucene.Net.Search.Query query = parser.Parse(luceneQuery);
            return Query(query);
        }
    }
}
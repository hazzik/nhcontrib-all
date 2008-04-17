using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Burrow;

namespace ProjectBase.Data.NHibernateSessionMgmt
{
    public class NHibernateSessionManagerImplBurrow : INHibernateSessionManager
    {
        /// <summary>
        /// Allows you to register an interceptor on a new session.  This may not be called if there is already
        /// an open session attached to the HttpContext.  If you have an interceptor to be used, modify
        /// the HttpModule to call this before calling BeginTransaction().
        /// </summary>
        public void RegisterInterceptor(IInterceptor interceptor)
        {
            throw new NotImplementedException();
        }

        public ISession GetSession()
        {
            BurrowFramework facade = new BurrowFramework();
            return facade.GetSession();
        }

        /// <summary>
        /// Flushes anything left in the session and closes the connection.
        /// </summary>
        public void CloseSession()
        {
            ISession session = GetSession();
            if (session != null && session.IsOpen)
            {
                session.Flush();
                session.Close();
            }
        }

        public ITransaction BeginTransaction()
        {
            ISession session = GetSession();

            ITransaction transaction=null;
            if (session != null && session.IsOpen)
            {
                transaction = session.Transaction;
                if (transaction == null || !transaction.IsActive)
                    transaction = session.BeginTransaction();
            }
            return transaction;
        }

        public void CommitTransaction()
        {
            ISession session = GetSession();

            ITransaction transaction;
            if (session != null && session.IsOpen)
            {
                transaction = session.Transaction;
                if (transaction != null && transaction.IsActive)
                    transaction.Commit();
            }
        }

        public bool HasOpenTransaction()
        {
            ISession session = GetSession();

            ITransaction transaction;
            if (session != null && session.IsOpen)
            {
                transaction = session.Transaction;
                if (transaction != null && transaction.IsActive)
                    return true;
            }
            return false;
        }

        public void RollbackTransaction()
        {
            ISession session = GetSession();

            ITransaction transaction;
            if (session != null && session.IsOpen)
            {
                transaction = session.Transaction;
                if (transaction != null && transaction.IsActive)
                    transaction.Rollback();
            }
        }
    }
}

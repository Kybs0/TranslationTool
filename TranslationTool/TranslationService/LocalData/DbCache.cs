using System;
using System.Data;
using System.Diagnostics;
using LinqToDB.DataProvider.SQLite;

namespace Translation.Business
{
    public class DbCache<TDB> where TDB : LinqToDB.Data.DataConnection, new()
    {
        private readonly string _connectionString;
        private readonly Func<string> _getConnectionStringFunc;

        private string ConnectionString
            => string.IsNullOrEmpty(_connectionString) ? _getConnectionStringFunc() : _connectionString;

        public DbCache(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbCache(Func<string> getConnectionStringFunc)
        {
            _getConnectionStringFunc = getConnectionStringFunc;
        }

        public void Execute(Action<TDB> execute)
        {
            TDB db = null;
            Exception exception = null;
            try
            {
                db = (TDB)Activator.CreateInstance(typeof(TDB), new SQLiteDataProvider(), ConnectionString);
                execute(db);
            }
            catch (Exception ex)
            {
                exception = ex;
                Debug.WriteLine(exception);
            }
            finally
            {
                db?.Dispose();
            }
            HandlerException(exception);
        }

        public bool ExecuteTransaction(Action<TDB> execute)
        {
            TDB db = null;
            Exception exception = null;
            try
            {
                db = (TDB)Activator.CreateInstance(typeof(TDB), new SQLiteDataProvider(), ConnectionString);
                using (var transaction = db.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        execute(db);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        exception = ex;
                        Debug.WriteLine(exception);
                        return false;
                    }
                    transaction.Commit();
                    return true;
                }
            }
            finally
            {
                db?.Dispose();
                HandlerException(exception);
            }
        }

        public TReturn Execute<TReturn>(Func<TDB, TReturn> execute)
        {
            TDB db = null;
            Exception exception = null;
            try
            {
                db = (TDB)Activator.CreateInstance(typeof(TDB), new SQLiteDataProvider(), ConnectionString);
                return execute(db);
            }
            catch (Exception ex)
            {
                exception = ex;
                Debug.WriteLine(exception);
                return default(TReturn);
            }
            finally
            {
                db?.Dispose();
                HandlerException(exception);
            }
        }

        private void HandlerException(Exception ex)
        {
            if (ex == null)
            {
                return;
            }

            try
            {
                HandlerExecuteException(ex);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        protected virtual void HandlerExecuteException(Exception ex)
        {

        }
    }
}

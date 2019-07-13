using System;
using System.Data;
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
            try
            {
                db = (TDB)Activator.CreateInstance(typeof(TDB), new SQLiteDataProvider(), ConnectionString);
                execute(db);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                db?.Dispose();
            }
        }

        public bool ExecuteTransaction(Action<TDB> execute)
        {
            TDB db = null;
            try
            {
                db = (TDB)Activator.CreateInstance(typeof(TDB), new SQLiteDataProvider(), ConnectionString);
                using (var transaction = db.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        execute(db);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                    transaction.Commit();
                    return true;
                }
            }
            finally
            {
                db?.Dispose();
            }
        }

        public TReturn Execute<TReturn>(Func<TDB, TReturn> execute)
        {
            TDB db = null;
            try
            {
                db = (TDB)Activator.CreateInstance(typeof(TDB), new SQLiteDataProvider(), ConnectionString);
                return execute(db);
            }
            catch (Exception ex)
            {
                return default(TReturn);
            }
            finally
            {
                db?.Dispose();
            }
        }
    }
}

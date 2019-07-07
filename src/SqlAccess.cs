using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SqlBuilder.DataServices
{
  public class SqlAccess : ISqlQuery, ISqlExecute
  {
    public SqlAccess(IConnectionFactory connectionFactory, IRetry sqlRetry)
    {
      _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
      _sqlRetry = sqlRetry ?? throw new ArgumentNullException(nameof(sqlRetry));
    }

    public SqlAccess(IConnectionFactory connectionFactory)
      : this(connectionFactory, new SqlRetry()) { }

    public IEnumerable<T> Query<T>(IDbTransaction transaction, string command, object param = null, CommandType commandType = CommandType.StoredProcedure, Action<IDbConnection> onExecute = null)
    {
      return _sqlRetry.Retry(() => transaction.Connection.Query<T>(command, param.ToParameters(), transaction, commandType: commandType));
    }

    public IEnumerable<T> Query<T>(string command, object param = null, CommandType commandType = CommandType.StoredProcedure, Action<IDbConnection> onExecute = null)
    {
      return Query((connection) =>
      {
        onExecute?.Invoke(connection);
        return connection.Query<T>(command, param.ToParameters(), commandType: commandType);
      });
    }

    public IEnumerable<T> QueryWithTransaction<T>(string command, object param = null, CommandType commandType = CommandType.StoredProcedure)
    {
      return QueryWithTransaction((transaction) => transaction.Connection.Query<T>(command, param.ToParameters(), transaction, commandType: commandType));
    }

    public IEnumerable<T> QueryWithTransaction<T>(Func<IDbTransaction, IEnumerable<T>> action)
    {
      IEnumerable<T> result;

      using (IDbTransaction transaction = _connectionFactory.CreateTransaction(true))
      {
        using (transaction.Connection)
        {
          try
          {
            result = action(transaction);
            transaction.Commit();
          }
          catch
          {
            transaction.Rollback();
            throw;
          }
        }
      }

      return result;
    }

    public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string command, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = DefaultSplitOn, CommandType commandType = CommandType.StoredProcedure)
    {
      return Query((connection) => connection.Query(command, map, param.ToParameters(), splitOn: splitOn, commandType: commandType));
    }

    public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string command, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, string splitOn = DefaultSplitOn, CommandType commandType = CommandType.StoredProcedure)
    {
      return Query((connection) => connection.Query(command, map, param.ToParameters(), splitOn: splitOn, commandType: commandType));
    }

    public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string command, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, string splitOn = DefaultSplitOn, CommandType commandType = CommandType.StoredProcedure)
    {
      return Query((connection) => connection.Query(command, map, param.ToParameters(), splitOn: splitOn, commandType: commandType));
    }

    public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string command, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, string splitOn = DefaultSplitOn, CommandType commandType = CommandType.StoredProcedure)
    {
      return Query((connection) => connection.Query(command, map, param.ToParameters(), splitOn: splitOn, commandType: commandType));
    }

    public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string command, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, string splitOn = DefaultSplitOn, CommandType commandType = CommandType.StoredProcedure)
    {
      return Query((connection) => connection.Query(command, map, param.ToParameters(), splitOn: splitOn, commandType: commandType));
    }

    public IEnumerable<dynamic> Query(string command, object param = null, CommandType commandType = CommandType.StoredProcedure)
    {
      return Query<dynamic>(command, param, commandType);
    }

    public IGridReader QueryMultiple(string command, object param = null, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null, Action<IDbConnection> onExecute = null)
    {
      return _sqlRetry.Retry(() =>
      {
        IDbConnection connection = _connectionFactory.CreateConnection(true);
        onExecute?.Invoke(connection);
        return new GridReaderWrapper(connection, command, param, commandType, commandTimeout);
      });
    }

    public T Query<T>(Func<IDbConnection, T> action)
    {
      return _sqlRetry.Retry(() =>
      {
        using (IDbConnection connection = _connectionFactory.CreateConnection(true))
        {
          return action(connection);
        }
      });
    }

    public int Execute(IDbTransaction transaction, string command, object param = null, CommandType commandType = CommandType.StoredProcedure)
    {
      return _sqlRetry.Retry(() =>
      {
        using (transaction.Connection)
        {
          using (transaction)
          {
            return transaction.Connection.Execute(command, param.ToParameters(), transaction, commandType: commandType);
          }
        }
      });
    }

    public int Execute(string command, object param = null, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null)
    {
      using (IDbConnection connection = _connectionFactory.CreateConnection(true))
      {
        return connection.Execute(command, param.ToParameters(), commandType: commandType, commandTimeout: commandTimeout);
      }
    }

    public void Execute(Action<IDbConnection> action)
    {
      _sqlRetry.Retry(() =>
      {
        using (IDbConnection connection = _connectionFactory.CreateConnection(true))
        {
          action(connection);
        }
      });
    }

    public int ExecuteWithTransaction(string command, object param = null, CommandType commandType = CommandType.StoredProcedure)
    {
      return _sqlRetry.Retry(() =>
      {
        using (IDbTransaction transaction = _connectionFactory.CreateTransaction(true))
        {
          using (transaction.Connection)
          {
            try
            {
              int result = transaction.Connection.Execute(command, param.ToParameters(), transaction, commandType: commandType);
              transaction.Commit();
              return result;
            }
            catch
            {
              transaction.Rollback();
              throw;
            }
          }
        }
      });
    }

    public void ExecuteWithTransaction(Action<IDbTransaction> action)
    {
      _sqlRetry.Retry(() =>
      {
        using (IDbTransaction transaction = _connectionFactory.CreateTransaction(true))
        {
          using (transaction.Connection)
          {
            try
            {
              action(transaction);
              transaction.Commit();
            }
            catch
            {
              transaction.Rollback();
              throw;
            }
          }
        }
      });
    }

    public const string DefaultSplitOn = "Id";
    
    private readonly IConnectionFactory _connectionFactory;

    private readonly IRetry _sqlRetry;
  }
}
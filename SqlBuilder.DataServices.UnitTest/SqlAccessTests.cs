using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;

namespace SqlBuilder.DataServices.UnitTest
{
  [TestClass]
  public class SqlAccessTests
  {
    public SqlAccessTests()
    {
      _connectionFactory = A.Fake<IConnectionFactory>();
      _connection = A.Fake<IDbConnection>();
      _transaction = A.Fake<IDbTransaction>();
      _sqlRetry = A.Fake<IRetry>();

      A.CallTo(() => _sqlRetry.Retry(A<Action>.Ignored)).Invokes((Action action) => action());
      A.CallTo(() => _connectionFactory.CreateConnection(A<bool>.Ignored)).Returns(_connection);
      A.CallTo(() => _connectionFactory.CreateTransaction(_connection)).Returns(_transaction);
      A.CallTo(() => _connectionFactory.CreateTransaction(A<bool>.Ignored)).Returns(_transaction);

      _access = new SqlAccess(_connectionFactory, _sqlRetry);
    }

    [TestMethod]
    public void TestExecuteCallsCreateConnectionOnce()
    {
      _access.Execute((c) => { });

      A.CallTo(() => _connectionFactory.CreateConnection(true)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void TestExecuteWithTransactionCallsCreateConnectionOnce()
    {
      _access.ExecuteWithTransaction((c) => { });

      A.CallTo(() => _connectionFactory.CreateTransaction(true)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void TestqueryCallsCreateConnectionOnce()
    {
      A.CallTo(() => _sqlRetry.Retry(A<Func<IEnumerable<dynamic>>>.Ignored)).Invokes((Func<IEnumerable<dynamic>> fn) => fn());

      _access.Query((c) => (IEnumerable<dynamic>)null);

      A.CallTo(() => _connectionFactory.CreateConnection(A<bool>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void TestExecuteDisposesTheConnection()
    {
      _access.Execute((c) => { });

      A.CallTo(() => _connection.Dispose()).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void TestExecuteDisposesTheConnectionWhenAnExceptionOccurs()
    {
      try
      {
        _access.Execute((c) =>
        {
          throw new Exception();
        });
      }
      catch { }

      A.CallTo(() => _connection.Dispose()).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void TestExecuteDisposesTheConnectionAndCommitsTheTransaction()
    {
      _access.ExecuteWithTransaction((t) => {  });

      A.CallTo(() => _transaction.Commit()).MustHaveHappenedOnceExactly();
      A.CallTo(() => _transaction.Connection.Dispose()).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void TestExecuteDisposesTheConnectionAndRollsBackTheTransactionWhenAnExceptionOccurs()
    {
      try
      {
        _access.ExecuteWithTransaction((t) =>
        {
          throw new Exception();
        });
      }
      catch { }

      A.CallTo(() => _transaction.Connection.Dispose()).MustHaveHappenedOnceExactly();
      A.CallTo(() => _transaction.Rollback()).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void TestQueryDisposesTheConnectionWhenAnExceptionOccurs()
    {
      A.CallTo(() => _sqlRetry.Retry(A<Func<IEnumerable<dynamic>>>.Ignored)).Invokes((Func<IEnumerable<dynamic>> fn) => fn());

      try
      {
        _access.Query((c) => 
        {
          return (IEnumerable<dynamic>)null;
        });
      }
      catch { }

      A.CallTo(() => _connection.Dispose()).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void QueryMultiple_uses_commandTimeout()
    {
      const int timeout = 999;

      _access.QueryMultiple("command", null, commandTimeout: timeout);

      // can't test :(
    }

    private readonly IDbConnection _connection;

    private readonly IDbTransaction _transaction;

    private readonly IConnectionFactory _connectionFactory;

    private readonly SqlAccess _access;

    private readonly IRetry _sqlRetry;
  }
}

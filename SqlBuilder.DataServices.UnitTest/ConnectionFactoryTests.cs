using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace SqlBuilder.DataServices.UnitTest
{
  [TestClass]
  public class ConnectionFactoryTests
  {
    public ConnectionFactoryTests()
    {
      _connection = A.Fake<IDbConnection>();
      _factory = () => _connection;
    }

    [TestMethod]
    public void CreateConnection_opens_connection_when_open_is_true_and_connection_is_closed()
    {
      A.CallTo(() => _connection.State).Returns(ConnectionState.Closed);

      new ConnectionFactory(_factory).CreateConnection(true);

      A.CallTo(() => _connection.Open()).MustHaveHappened();
    }

    [TestMethod]
    public void CreateConnection_does_not_call_open_on_connection_when_open_is_true_and_connection_already_open()
    {
      A.CallTo(() => _connection.State).Returns(ConnectionState.Open);

      new ConnectionFactory(_factory).CreateConnection(true);

      A.CallTo(() => _connection.Open()).MustNotHaveHappened();
    }

    [TestMethod]
    public void CreateConnection_does_not_call_open_on_connection_when_open_is_false()
    {
      new ConnectionFactory(_factory).CreateConnection(false);

      A.CallTo(() => _connection.Open()).MustNotHaveHappened();
    }

    [TestMethod]
    public void CreateConnection_opens_connection_by_default()
    {
      new ConnectionFactory(_factory).CreateConnection();

      A.CallTo(() => _connection.Open()).MustHaveHappened();
    }

    private readonly IDbConnection _connection;

    private readonly Func<IDbConnection> _factory;
  }
}
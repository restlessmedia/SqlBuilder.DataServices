using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace SqlBuilder.DataServices.UnitTest
{
  [TestClass]
  public class ConnectionFactoryTests
  {
    [TestMethod]
    public void CreateConnection_opens_connection_when_open_is_true_and_connection_is_closed()
    {
      IDbConnection dbConnection = A.Fake<IDbConnection>();
      A.CallTo(() => dbConnection.State).Returns(ConnectionState.Closed);

      new ConnectionFactory(dbConnection).CreateConnection(true);

      A.CallTo(() => dbConnection.Open()).MustHaveHappened();
    }

    [TestMethod]
    public void CreateConnection_does_not_call_open_on_connection_when_open_is_true_and_connection_already_open()
    {
      IDbConnection dbConnection = A.Fake<IDbConnection>();
      A.CallTo(() => dbConnection.State).Returns(ConnectionState.Open);

      new ConnectionFactory(dbConnection).CreateConnection(true);

      A.CallTo(() => dbConnection.Open()).MustNotHaveHappened();
    }

    [TestMethod]
    public void CreateConnection_does_not_call_open_on_connection_when_open_is_false()
    {
      IDbConnection dbConnection = A.Fake<IDbConnection>();

      new ConnectionFactory(dbConnection).CreateConnection(false);

      A.CallTo(() => dbConnection.Open()).MustNotHaveHappened();
    }
  }
}
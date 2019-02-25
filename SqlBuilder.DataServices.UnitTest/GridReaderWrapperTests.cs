using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace SqlBuilder.DataServices.UnitTest
{
  [TestClass]
  public class GridReaderWrapperTests
  {
    [TestMethod]
    public void TestConnectionIsClosed()
    {
      IDbConnection connection = A.Fake<IDbConnection>();
      string command = "test-command";
      object param = new { };

      A.CallTo(() => connection.State).Returns(ConnectionState.Open);

      using (GridReaderWrapper wrapper = new GridReaderWrapper(connection, command, new { })) { }

      A.CallTo(() => connection.Close()).MustHaveHappened();
    }
  }
}
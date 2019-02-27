using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace SqlBuilder.DataServices.UnitTest
{
  [TestClass]
  public class IExecuteExtensionsTests
  {
    [TestMethod]
    public void Execute_excutes_with_arguments()
    {
      ISqlExecute sqlExecute = A.Fake<ISqlExecute>();
      SqlText sqlText = A.Fake<SqlText>();
      ParameterCollection parameters = new ParameterCollection();
      const string sql = "select 1";

      A.CallTo(() => sqlText.Sql()).Returns(sql);
      A.CallTo(() => sqlText.Parameters).Returns(parameters);

      sqlExecute.Execute(sqlText);

      A.CallTo(() => sqlExecute.Execute(sql, parameters, CommandType.Text, null)).MustHaveHappenedOnceExactly();
    }
  }
}

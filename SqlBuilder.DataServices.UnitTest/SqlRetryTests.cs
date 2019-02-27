using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SqlBuilder.DataServices.UnitTest
{
  [TestClass]
  public class SqlRetryTests
  {
    //this hangs the test runner
    //[TestMethod]
    //[DataRow(1205, 3, 1)]
    public void ad(int errorCode, int times, int seconds)
    {
      int count = 0;

      try
      {
        Action action = () =>
        {
          count++;
          throw ObjectHelper.CreateSqlException("", errorCode);
        };
        SqlRetry sqlRetry = new SqlRetry();
        sqlRetry.Retry(action, times, TimeSpan.FromSeconds(seconds));
      }
      finally
      {
        count.MustBe(times);
      }
    }
  }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SqlBuilder.DataServices.UnitTest
{
  [TestClass]
  public class TransientErrorDetectionStrategyTests
  {
    [TestMethod]
    public void IsTransient_returns_true_when_exception_is_timeout()
    {
      new TransientErrorDetectionStrategy().IsTransient(new TimeoutException()).MustBeTrue();
    }

    [TestMethod]
    [DataRow(1205, true)] // deadlock
    [DataRow(-2, true)] // timeout
    [DataRow(64, false)] // login error, not transient

    public void IsTransient_returns_true_when_sql_error_is_deadlock_or_timeout(int code, bool expected)
    {
      new TransientErrorDetectionStrategy().IsTransient(ObjectHelper.CreateSqlException("", code)).MustBe(expected);
    }
  }
}
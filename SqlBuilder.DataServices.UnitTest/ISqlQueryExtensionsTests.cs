using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.DataServices.UnitTest
{
  [TestClass]
  public class ISqlQueryExtensionsTests
  {
    [TestMethod]
    public void QueryPage_excutes_with_arguments()
    {
      // set-up
      ISqlQuery sqlQuery = A.Fake<ISqlQuery>();
      ParameterCollection parameters = new ParameterCollection();
      Select select = new Select();

      // call
      sqlQuery.QueryPage<DateTime>(select);

      // assert
      A.CallTo(() => sqlQuery.QueryMultiple(select.Sql(), select.Parameters, CommandType.Text, A<int?>.Ignored, A<Action<IDbConnection>>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void QueryPage_gets_count_from_dataset_when_includeCount_is_true()
    {
      // set-up
      ISqlQuery sqlQuery = A.Fake<ISqlQuery>();
      Select select = new Select(includeCount: true);
      IGridReader gridReader = A.Fake<IGridReader>();
      DateTime now = DateTime.Now;

      A.CallTo(() => gridReader.Read<DateTime>()).Returns(new[] { now });
      A.CallTo(() => gridReader.Read<int>()).Returns(new[] { 5 });
      A.CallTo(() => sqlQuery.QueryMultiple(select.Sql(), select.Parameters, CommandType.Text, A<int?>.Ignored, A<Action<IDbConnection>>.Ignored)).Returns(gridReader);

      // call
      DataPage<DateTime> dataPage = sqlQuery.QueryPage<DateTime>(select);

      // assert
      dataPage.Count.MustBe(5);
      dataPage.Data.First().MustBe(now);
    }

    [TestMethod]
    public void QueryPage_gets_count_from_data_count_when_includeCount_is_false()
    {
      // set-up
      ISqlQuery sqlQuery = A.Fake<ISqlQuery>();
      Select select = new Select(includeCount: false);
      IGridReader gridReader = A.Fake<IGridReader>();
      DateTime now = DateTime.Now;

      A.CallTo(() => gridReader.Read<DateTime>()).Returns(new[] { now });
      A.CallTo(() => gridReader.Read<int>()).Returns(new[] { 5 });
      A.CallTo(() => sqlQuery.QueryMultiple(select.Sql(), select.Parameters, CommandType.Text, A<int?>.Ignored, A<Action<IDbConnection>>.Ignored)).Returns(gridReader);

      // call
      DataPage<DateTime> dataPage = sqlQuery.QueryPage<DateTime>(select);

      // assert
      dataPage.Count.MustBe(1);
      dataPage.Data.First().MustBe(now);
    }

    [TestMethod]
    public void Execute_excutes_with_arguments()
    {
      // set-up
      ISqlQuery sqlQuery = A.Fake<ISqlQuery>();
      SqlText sqlText = A.Fake<SqlText>();
      ParameterCollection parameters = new ParameterCollection();
      const string sql = "select 1";

      A.CallTo(() => sqlText.Sql()).Returns(sql);
      A.CallTo(() => sqlText.Parameters).Returns(parameters);

      // call
      sqlQuery.Query<DateTime>(sqlText);

      // assert
      A.CallTo(() => sqlQuery.Query<DateTime>(sql, parameters, CommandType.Text, null)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void QueryMultiple_executes_with_arguments()
    {
      // set-up
      ISqlQuery sqlQuery = A.Fake<ISqlQuery>();
      SqlText sqlText = A.Fake<SqlText>();
      ParameterCollection parameters = new ParameterCollection();
      const string sql = "select 1";

      A.CallTo(() => sqlText.Sql()).Returns(sql);
      A.CallTo(() => sqlText.Parameters).Returns(parameters);

      // call tolist because of the yield delayed evaluation
      sqlQuery.QueryMultiple<int, DateTime>(sqlText).ToList();

      // assert
      A.CallTo(() => sqlQuery.QueryMultiple(sqlText.Sql(), sqlText.Parameters, CommandType.Text, A<int?>.Ignored, A<Action<IDbConnection>>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void QueryMultiple_executes_with_type_arguments()
    {
      QueryMultipleTest<DateTime, int, object, object>(DateTime.Now, 4);
      QueryMultipleTest<DateTime, int, string, object>(DateTime.Now, 4, "test");
      QueryMultipleTest(DateTime.Now, 4, "test", false);
    }

    [TestMethod]
    public void Query_executes_connection_callback()
    {
      // set-up
      ISqlQuery sqlQuery = A.Fake<ISqlQuery>();
      SqlText sqlText = A.Fake<SqlText>();
      const string sql = "select 1";

      A.CallTo(() => sqlText.Sql()).Returns(sql);
      Action<IDbConnection> callback = A.Fake<Action<IDbConnection>>();
      A.CallTo(() => sqlQuery.Query<DateTime>(sql, A<object>.Ignored, CommandType.Text, callback)).Invokes(() => callback(null));

      // call
      sqlQuery.Query<DateTime>(sqlText, callback);

      // assert
      A.CallTo(() => callback(A<IDbConnection>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void QueryMultiple_executes_connection_callback()
    {
      // set-up
      ISqlQuery sqlQuery = A.Fake<ISqlQuery>();
      SqlText sqlText = A.Fake<SqlText>();
      const string sql = "select 1";

      A.CallTo(() => sqlText.Sql()).Returns(sql);
      Action<IDbConnection> callback = A.Fake<Action<IDbConnection>>();
      A.CallTo(() => sqlQuery.QueryMultiple(sql, A<object>.Ignored, CommandType.Text, A<int?>.Ignored, callback)).Invokes(() => callback(null));

      // call tolist because of the yield delayed evaluation
      sqlQuery.QueryMultiple<string, int>(sqlText, callback).ToList();

      // assert
      A.CallTo(() => callback(A<IDbConnection>.Ignored)).MustHaveHappenedOnceExactly();
    }

    private void QueryMultipleTest<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3 = default(T3), T4 value4 = default(T4))
    {
      // set-up
      ISqlQuery sqlQuery = A.Fake<ISqlQuery>();
      SqlText sqlText = A.Fake<SqlText>();
      IGridReader gridReader = A.Fake<IGridReader>();

      if (value1 != null)
      {
        A.CallTo(() => gridReader.Read<T1>()).Returns(new[] { value1 });
      }

      if (value2 != null)
      {
        A.CallTo(() => gridReader.Read<T2>()).Returns(new[] { value2 });
      }

      if (value3 != null)
      {
        A.CallTo(() => gridReader.Read<T3>()).Returns(new[] { value3 });
      }

      if (value4 != null)
      {
        A.CallTo(() => gridReader.Read<T4>()).Returns(new[] { value4 });
      }

      A.CallTo(() => sqlQuery.QueryMultiple(A<string>.Ignored, A<ParameterCollection>.Ignored, CommandType.Text, A<int?>.Ignored, A<Action<IDbConnection>>.Ignored)).Returns(gridReader);

      IEnumerable<IEnumerable> result;

      // call and assert

      if (value4 != null)
      {
        result = sqlQuery.QueryMultiple<T1, T2, T3, T4>(sqlText);
        ((IEnumerable<T1>)result.First()).First().MustBe(value1);
        ((IEnumerable<T2>)result.Skip(1).First()).First().MustBe(value2);
        ((IEnumerable<T3>)result.Skip(2).First()).First().MustBe(value3);
        ((IEnumerable<T4>)result.Skip(3).First()).First().MustBe(value4);
      }
      else if (value3 != null)
      {
        result = sqlQuery.QueryMultiple<T1, T2, T3>(sqlText);
        ((IEnumerable<T1>)result.First()).First().MustBe(value1);
        ((IEnumerable<T2>)result.Skip(1).First()).First().MustBe(value2);
        ((IEnumerable<T3>)result.Skip(2).First()).First().MustBe(value3);
      }
      else
      {
        result = sqlQuery.QueryMultiple<T1, T2>(sqlText);
        ((IEnumerable<T1>)result.First()).First().MustBe(value1);
        ((IEnumerable<T2>)result.Skip(1).First()).First().MustBe(value2);
      }
    }
  }
}
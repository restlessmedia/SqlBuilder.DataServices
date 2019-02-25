using System;
using System.Collections.Generic;
using System.Data;

namespace SqlBuilder.DataServices
{
  public interface ISqlQuery
  {
    IEnumerable<T> Query<T>(string command, object param = null, CommandType commandType = CommandType.StoredProcedure, Action<IDbConnection> onExecute = null);

    IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string command, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id", CommandType commandType = CommandType.StoredProcedure);

    IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string command, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, string splitOn = "Id", CommandType commandType = CommandType.StoredProcedure);

    IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string command, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, string splitOn = "Id", CommandType commandType = CommandType.StoredProcedure);

    IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string command, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, string splitOn = "Id", CommandType commandType = CommandType.StoredProcedure);

    IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string command, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, string splitOn = "Id", CommandType commandType = CommandType.StoredProcedure);

    IEnumerable<dynamic> Query(string command, object param = null, CommandType commandType = CommandType.StoredProcedure);

    IGridReader QueryMultiple(string command, object param = null, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null, Action<IDbConnection> onExecute = null);

    T Query<T>(Func<IDbConnection, T> action);
  }
}
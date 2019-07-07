using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SqlBuilder.DataServices
{
  public static class ISqlQueryExtensions
  {
    /// <summary>
    /// Returns a data page.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sqlQuery"></param>
    /// <param name="select"></param>
    /// <param name="onExecute"></param>
    /// <returns></returns>
    public static DataPage<T> QueryPage<T>(this ISqlQuery sqlQuery, Select select, Action<IDbConnection> onExecute = null)
    {
      IEnumerable<IEnumerable> enumerable = QueryMultiple<T, int>(sqlQuery, select, onExecute);
      IEnumerable<T> data = (IEnumerable<T>)enumerable.First();

      if (select.IncludeCount())
      {
        int count = ((IEnumerable<int>)enumerable.Skip(1).First()).First();
        return new DataPage<T>(data, count);
      }

      return new DataPage<T>(data);
    }

    /// <summary>
    /// Returns dynamic query.
    /// </summary>
    /// <param name="sqlQuery"></param>
    /// <param name="sqlText"></param>
    /// <param name="onExecute"></param>
    /// <returns></returns>
    public static IEnumerable<dynamic> QueryDynamic(this ISqlQuery sqlQuery, SqlText sqlText, Action<IDbConnection> onExecute = null)
    {
      return sqlQuery.Query<dynamic>(sqlText.Sql(), sqlText.Parameters, commandType: CommandType.Text, onExecute: onExecute);
    }

    /// <summary>
    /// Returns multiple recordsets for the provided types.
    /// </summary>
    /// <typeparam name="TFirst"></typeparam>
    /// <typeparam name="TSecond"></typeparam>
    /// <param name="sqlQuery"></param>
    /// <param name="sqlText"></param>
    /// <param name="onExecute"></param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable> QueryMultiple<TFirst, TSecond>(this ISqlQuery sqlQuery, SqlText sqlText, Action<IDbConnection> onExecute = null)
    {
      using (IGridReader gridReader = sqlQuery.QueryMultiple(sqlText.Sql(), sqlText.Parameters, commandType: CommandType.Text, onExecute: onExecute))
      {
        yield return gridReader.Read<TFirst>();
        yield return gridReader.Read<TSecond>();
      }
    }

    /// <summary>
    /// Returns multiple recordsets for the provided types.
    /// </summary>
    /// <typeparam name="TFirst"></typeparam>
    /// <typeparam name="TSecond"></typeparam>
    /// <typeparam name="TThird"></typeparam>
    /// <param name="sqlQuery"></param>
    /// <param name="sqlText"></param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable> QueryMultiple<TFirst, TSecond, TThird>(this ISqlQuery sqlQuery, SqlText sqlText)
    {
      using (IGridReader gridReader = sqlQuery.QueryMultiple(sqlText.Sql(), sqlText.Parameters, commandType: CommandType.Text))
      {
        yield return gridReader.Read<TFirst>();
        yield return gridReader.Read<TSecond>();
        yield return gridReader.Read<TThird>();
      }
    }

    /// <summary>
    /// Returns multiple recordsets for the provided types.
    /// </summary>
    /// <typeparam name="TFirst"></typeparam>
    /// <typeparam name="TSecond"></typeparam>
    /// <typeparam name="TThird"></typeparam>
    /// <typeparam name="TFourth"></typeparam>
    /// <param name="sqlQuery"></param>
    /// <param name="sqlText"></param>
    /// <param name="onExecute"></param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable> QueryMultiple<TFirst, TSecond, TThird, TFourth>(this ISqlQuery sqlQuery, SqlText sqlText, Action<IDbConnection> onExecute = null)
    {
      using (IGridReader gridReader = sqlQuery.QueryMultiple(sqlText.Sql(), sqlText.Parameters, CommandType.Text, onExecute: onExecute))
      {
        yield return gridReader.Read<TFirst>();
        yield return gridReader.Read<TSecond>();
        yield return gridReader.Read<TThird>();
        yield return gridReader.Read<TFourth>();
      }
    }

    /// <summary>
    /// Returns a query based on the <see cref="SqlText"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sqlQuery"></param>
    /// <param name="sqlText"></param>
    /// <param name="onExecute"></param>
    /// <returns></returns>
    public static IEnumerable<T> Query<T>(this ISqlQuery sqlQuery, SqlText sqlText, Action<IDbConnection> onExecute = null)
    {
      string sql = sqlText.Sql();

      if (string.IsNullOrEmpty(sql))
      {
        return Enumerable.Empty<T>();
      }

      return sqlQuery.Query<T>(sql, sqlText.Parameters, CommandType.Text, onExecute);
    }

    /// <summary>
    /// Returns the first record from a query based on <see cref="SqlText"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sqlQuery"></param>
    /// <param name="sqlText"></param>
    /// <returns></returns>
    public static T FirstOrDefault<T>(this ISqlQuery sqlQuery, SqlText sqlText)
    {
      return Query<T>(sqlQuery, sqlText).FirstOrDefault();
    }
  }
}
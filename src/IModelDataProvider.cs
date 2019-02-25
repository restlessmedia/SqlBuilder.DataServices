using System.Collections.Generic;

namespace SqlBuilder.DataServices
{
  public interface IModelDataProvider<TDataModel> : ISqlQuery, ISqlExecute
    where TDataModel : DataModel, new()
  {
    IEnumerable<TDataModel> All();

    TDataModel ByKey(object key);

    /// <summary>
    /// Executes a query.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    IEnumerable<TDataModel> Query(string sql, object param = null);

    /// <summary>
    /// Executes a query.
    /// </summary>
    /// <param name="select"></param>
    /// <returns></returns>
    IEnumerable<TDataModel> Query(Select select);

    bool Exists(object key);

    Select<TDataModel> NewSelect();
  }
}
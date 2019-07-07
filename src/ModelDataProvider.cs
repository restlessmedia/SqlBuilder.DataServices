using System;
using System.Collections.Generic;

namespace SqlBuilder.DataServices
{
  internal class ModelDataProvider<TDataModel> : SqlAccess, IModelDataProvider<TDataModel>
    where TDataModel : DataModel, new()
  {
    public ModelDataProvider(IConnectionFactory connectionFactory)
      : base(connectionFactory)
    {
      _definition = ModelDefinition.GetDefinition<TDataModel>();
    }

    public IEnumerable<TDataModel> All()
    {
      Select<TDataModel> select = new Select<TDataModel>();
      return this.Query<TDataModel>(select);
    }

    public TDataModel ByKey(object key)
    {
      CheckKey();

      Select<TDataModel> select = new Select<TDataModel>();
      string parameter = select.Parameters.Add(key);
      select.Where($"{_definition.Key}={parameter}");
      return this.FirstOrDefault<TDataModel>(select);
    }

    public IEnumerable<TDataModel> Query(string sql, object param = null)
    {
      return Query<TDataModel>(sql);
    }

    public IEnumerable<TDataModel> Query(Select select)
    {
      return this.Query<TDataModel>(select);
    }

    public bool Exists(object key)
    {
      CheckKey();

      Select<TDataModel> select = new Select<TDataModel>("1");
      string parameter = select.Parameters.Add(key);
      select.Where($"{_definition.Key}={parameter}");
      return this.FirstOrDefault<int?>(select).HasValue;
    }

    public Select<TDataModel> NewSelect()
    {
      return new Select<TDataModel>();
    }

    private void CheckKey()
    {
      if (string.IsNullOrEmpty(_definition.Key))
      {
        throw new InvalidOperationException($"The model for {_definition.GetTableName()} does not contain a member with a key.");
      }
    }

    private readonly ModelDefinition _definition;
  }
}
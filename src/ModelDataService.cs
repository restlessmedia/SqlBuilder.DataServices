using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SqlBuilder.DataServices
{
  internal class ModelDataService<TDataModel, TViewModel> : ModelDataService<TDataModel>, IModelDataService<TDataModel, TViewModel>
    where TDataModel : DataModel, new()
    where TViewModel : new()
  {
    public ModelDataService(IModelDataProvider<TDataModel> provider, IModelFactory<TDataModel, TViewModel> modelFactory)
      : base(provider)
    {
      ModelFactory = modelFactory ?? throw new ArgumentNullException(nameof(modelFactory));
    }

    /// <summary>
    /// Returns all records
    /// </summary>
    /// <returns></returns>
    public new IEnumerable<TViewModel> All()
    {
      return base.All().Select(ModelFactory.CreateViewModel);
    }

    /// <summary>
    /// Returns a single model with the given key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public new TViewModel ByKey(object key)
    {
      TDataModel dataModel = DataProvider.ByKey(key);

      if (dataModel != null)
      {
        return ModelFactory.CreateViewModel(dataModel);
      }

      return default(TViewModel);
    }

    /// <summary>
    /// Creates the model
    /// </summary>
    /// <param name="model"></param>
    public int? Create(TViewModel model)
    {
      TDataModel dataModel = ModelFactory.CreateDataModel(model);
      return Create(dataModel);
    }

    /// <summary>
    /// Updates the model
    /// </summary>
    /// <param name="key"></param>
    /// <param name="model"></param>
    public void Update(object key, TViewModel model)
    {
      TDataModel dataModel = ModelFactory.CreateDataModel(model);
      Update(key, dataModel);
    }

    /// <summary>
    /// Updates only the changed properties by checking the current values against the next ones.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="model"></param>
    public void UpdateChanged(object key, TViewModel model)
    {
      TDataModel next = ModelFactory.CreateDataModel(model);
      UpdateChanged(key, next);
    }

    public new IEnumerable<TViewModel> Query(Select select)
    {
      return DataProvider.Query(select).Select(x => ModelFactory.CreateViewModel(x));
    }

    public readonly IModelFactory<TDataModel, TViewModel> ModelFactory;
  }

  internal class ModelDataService<TDataModel> : ModelDataService, IModelDataService<TDataModel>
    where TDataModel : DataModel, new()
  {
    public ModelDataService(IModelDataProvider<TDataModel> dataProvider)
    {
      DataProvider = dataProvider;
      _definition = ModelDefinition.GetDefinition<TDataModel>();
    }

    /// <summary>
    /// Returns all records
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<TDataModel> All()
    {
      return DataProvider.All();
    }

    public virtual TDataModel ByKey(object key)
    {
      return DataProvider.ByKey(key);
    }

    /// <summary>
    /// Creates the data model and returns the new key value
    /// </summary>
    /// <param name="dataModel"></param>
    /// <returns></returns>
    public virtual int? Create(TDataModel dataModel)
    {
      Insert<TDataModel> insert = new Insert<TDataModel>(dataModel, true);
      return DataProvider.Query<int?>(insert, connection => TriggerOnCreate(this, connection)).FirstOrDefault();
    }

    /// <summary>
    /// Updates the data model
    /// </summary>
    /// <param name="key"></param>
    /// <param name="dataModel"></param>
    public virtual void Update(object key, TDataModel dataModel)
    {
      Update<TDataModel> update = new Update<TDataModel>(key, dataModel);
      DataProvider.Execute(update);
    }

    /// <summary>
    /// Deletes the record for the given key
    /// </summary>
    /// <param name="key"></param>
    public virtual void Delete(object key)
    {
      Delete<TDataModel> delete = new Delete<TDataModel>(key);
      DataProvider.Execute(delete);
    }

    /// <summary>
    /// If true, a record exists with the given key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual bool Exists(object key)
    {
      return DataProvider.Exists(key);
    }

    public virtual void UpdateChanged(object key, TDataModel next)
    {
      TDataModel current = DataProvider.ByKey(key);

      if (current == null)
      {
        // no previous record, return
        return;
      }

      Update<TDataModel> update = new Update<TDataModel>(key, current, next);
      DataProvider.Execute(update);
    }

    public virtual IEnumerable<TDataModel> Query(Select select)
    {
      return DataProvider.Query(select);
    }

    public virtual ModelDefinition Definition()
    {
      return _definition;
    }

    public IModelDataProvider<TDataModel> DataProvider { get; private set; }

    private readonly ModelDefinition _definition;
  }

  public class ModelDataService
  {
    internal static void TriggerOnCreate(object sender, IDbConnection dbConnection)
    {
      OnCreate?.Invoke(sender, dbConnection);
    }

    public static void ClearAllEvents()
    {
      if (OnCreate != null)
      {
        OnCreate = null;
      }
    }

    public static event CreateEventHandler OnCreate;
  }
}
using System.Collections.Generic;

namespace SqlBuilder.DataServices
{
  public interface IModelDataService<TDataModel, TViewModel> : IModelDataService<TDataModel>
    where TDataModel : DataModel, new()
    where TViewModel : new()
  {
    IEnumerable<TViewModel> All();

    TViewModel ByKey(object key);

    int? Create(TViewModel model);

    IEnumerable<TViewModel> Query(Select select);

    void Update(object key, TViewModel model);

    void UpdateChanged(object key, TViewModel model);
  }

  public interface IModelDataService<TDataModel>
    where TDataModel : DataModel, new()
  {
    IEnumerable<TDataModel> All();

    TDataModel ByKey(object key);

    int? Create(TDataModel dataModel);

    ModelDefinition Definition();

    void Delete(object key);

    bool Exists(object key);

    IEnumerable<TDataModel> Query(Select select);

    void Update(object key, TDataModel dataModel);

    void UpdateChanged(object key, TDataModel next);
  }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.DataServices.UnitTest
{
  public class Tests
  {
    /// <summary>
    /// Tests <see cref="ModelDataService<,>"/>
    /// </summary>
    [TestClass]
    public class ModelDataServiceWithDataViewTests
    {
      public ModelDataServiceWithDataViewTests()
      {
        _dataProvider = A.Fake<IModelDataProvider<TestDataModel>>();
        _modelFactory = A.Fake<IModelFactory<TestDataModel, TestViewModel>>();
        _dataService = new ModelDataService<TestDataModel, TestViewModel>(_dataProvider, _modelFactory);
      }

      [TestMethod]
      public void All_calls_provider()
      {
        _dataService.All();

        A.CallTo(() => _dataProvider.All()).MustHaveHappenedOnceExactly();
      }

      [TestMethod]
      public void All_creates_view_model()
      {
        TestDataModel testDataModel = new TestDataModel();

        A.CallTo(() => _dataProvider.All()).Returns(new[] { testDataModel });

        _dataService.All().ToList();

        A.CallTo(() => _modelFactory.CreateViewModel(testDataModel)).MustHaveHappenedOnceExactly();
      }

      [TestMethod]
      public void ByKey_calls_provider()
      {
        const int key = 2;

        _dataService.ByKey(key);

        A.CallTo(() => _dataProvider.ByKey(key)).MustHaveHappenedOnceExactly();
      }

      [TestMethod]
      public void ByKey_creates_view_model()
      {
        TestDataModel testDataModel = new TestDataModel();
        const int key = 2;

        A.CallTo(() => _dataProvider.ByKey(key)).Returns(testDataModel);

        _dataService.ByKey(key);

        A.CallTo(() => _modelFactory.CreateViewModel(testDataModel)).MustHaveHappenedOnceExactly();
      }

      [TestMethod]
      public void Create_calls_provider()
      {
        TestViewModel testViewModel = new TestViewModel();

        _dataService.Create(testViewModel);

        A.CallTo(() => _dataProvider.Query<int?>(A<string>.Ignored, A<ParameterCollection>.Ignored, CommandType.Text, A<Action<IDbConnection>>.Ignored)).MustHaveHappened();
      }

      [TestMethod]
      public void Create_creates_data_model()
      {
        TestViewModel testViewModel = new TestViewModel();

        _dataService.Create(testViewModel);

        A.CallTo(() => _modelFactory.CreateDataModel(testViewModel)).MustHaveHappenedOnceExactly();
      }

      [TestMethod]
      public void update_calls_provider()
      {
        object key = 5;
        TestViewModel testViewModel = new TestViewModel();

        _dataService.Update(key, testViewModel);

        A.CallTo(() => _dataProvider.Execute(A<string>.Ignored, A<ParameterCollection>.Ignored, CommandType.Text, null)).MustHaveHappenedOnceExactly();
      }

      [TestMethod]
      public void UpdateChanged_calls_provider_when_existing_record_found_and_model_is_different()
      {
        object key = 5;
        TestViewModel testViewModel = new TestViewModel { Title = "test1" };
        TestDataModel testDataModel = new TestDataModel { Title = "test2" };

        // mock the found record
        A.CallTo(() => _dataProvider.ByKey(key)).Returns(testDataModel);

        // this is for the conversion from view to data model
        A.CallTo(() => _modelFactory.CreateDataModel(testViewModel)).Returns(new TestDataModel { Title = testViewModel.Title });

        _dataService.UpdateChanged(key, testViewModel);

        A.CallTo(() => _dataProvider.Execute(A<string>.Ignored, A<ParameterCollection>.Ignored, CommandType.Text, null)).MustHaveHappenedOnceExactly();
      }

      [TestMethod]
      public void UpdateChanged_does_not_call_provider_when_existing_record_found_and_model_is_not_different()
      {
        object key = 5;
        TestViewModel testViewModel = new TestViewModel { Title = "test1" };
        TestDataModel testDataModel = new TestDataModel { Title = "test1" };

        // mock the found record
        A.CallTo(() => _dataProvider.ByKey(key)).Returns(testDataModel);

        // this is for the conversion from view to data model
        A.CallTo(() => _modelFactory.CreateDataModel(testViewModel)).Returns(new TestDataModel { Title = testViewModel.Title });

        _dataService.UpdateChanged(key, testViewModel);

        A.CallTo(() => _dataProvider.Execute(A<string>.Ignored, A<ParameterCollection>.Ignored, CommandType.Text, null)).MustNotHaveHappened();
      }

      [TestMethod]
      public void Query_calls_provider()
      {
        Select select = new Select();

        _dataService.Query(select);

        A.CallTo(() => _dataProvider.Query(select)).MustHaveHappenedOnceExactly();
      }

      private readonly IModelDataProvider<TestDataModel> _dataProvider;

      private readonly IModelFactory<TestDataModel, TestViewModel> _modelFactory;

      private readonly ModelDataService<TestDataModel, TestViewModel> _dataService;
    }

    /// <summary>
    /// Tests <see cref="ModelDataService<>"/>
    /// </summary>
    [TestClass]
    public class ModelDataServiceTests
    {
      public ModelDataServiceTests()
      {
        _dataProvider = A.Fake<IModelDataProvider<TestDataModel>>();
        _dataService = new ModelDataService<TestDataModel>(_dataProvider);
      }

      [TestMethod]
      public void All_calls_provider()
      {
        _dataService.All();

        A.CallTo(() => _dataProvider.All()).MustHaveHappenedOnceExactly();
      }

      [TestMethod]
      public void ByKey_calls_provider()
      {
        const int key = 2;

        _dataService.ByKey(key);

        A.CallTo(() => _dataProvider.ByKey(key)).MustHaveHappenedOnceExactly();
      }

      [TestMethod]
      public void Create_calls_provider()
      {
        TestDataModel testDataModel = new TestDataModel();

        _dataService.Create(testDataModel);

        A.CallTo(() => _dataProvider.Query<int?>(A<string>.Ignored, A<ParameterCollection>.Ignored, CommandType.Text, A<Action<IDbConnection>>.Ignored)).MustHaveHappened();
      }

      [TestMethod]
      public void update_calls_provider()
      {
        object key = 5;
        TestDataModel testDataModel = new TestDataModel();

        _dataService.Update(key, testDataModel);

        A.CallTo(() => _dataProvider.Execute(A<string>.Ignored, A<ParameterCollection>.Ignored, CommandType.Text, null)).MustHaveHappenedOnceExactly();
      }

      [TestMethod]
      public void delete_calls_provider()
      {
        object key = 5;

        _dataService.Delete(key);

        A.CallTo(() => _dataProvider.Execute(A<string>.Ignored, A<ParameterCollection>.Ignored, CommandType.Text, null)).MustHaveHappenedOnceExactly();
      }

      [TestMethod]
      public void exists_calls_provider()
      {
        object key = 5;

        _dataService.Exists(key);

        A.CallTo(() => _dataProvider.Exists(key)).MustHaveHappenedOnceExactly();
      }

      [TestMethod]
      public void UpdateChanged_calls_provider_when_existing_record_found_and_model_is_different()
      {
        object key = 5;
        TestDataModel current = new TestDataModel { Title = "test2" };
        TestDataModel next = new TestDataModel { Title = "test1" };

        // mock the found record
        A.CallTo(() => _dataProvider.ByKey(key)).Returns(next);

        _dataService.UpdateChanged(key, current);

        A.CallTo(() => _dataProvider.Execute(A<string>.Ignored, A<ParameterCollection>.Ignored, CommandType.Text, null)).MustHaveHappenedOnceExactly();
      }

      [TestMethod]
      public void UpdateChanged_does_not_call_provider_when_existing_record_found_and_model_is_not_different()
      {
        object key = 5;
        TestDataModel testDataModel = new TestDataModel { Title = "test1" };

        // mock the found record
        A.CallTo(() => _dataProvider.ByKey(key)).Returns(testDataModel);

        _dataService.UpdateChanged(key, testDataModel);

        A.CallTo(() => _dataProvider.Execute(A<string>.Ignored, A<ParameterCollection>.Ignored, CommandType.Text, null)).MustNotHaveHappened();
      }

      private readonly IModelDataProvider<TestDataModel> _dataProvider;

      private readonly ModelDataService<TestDataModel> _dataService;
    }

    public class TestDataModel : DataModel
    {
      [Key]
      public int Id { get; set; }

      // editable column
      public string Title { get; set; }
    }

    public class TestViewModel
    {
      [Key]
      public int Id { get; set; }

      // editable column
      public string Title { get; set; }
    }
  }
}
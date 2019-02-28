using System;
using System.ComponentModel.DataAnnotations;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.DataServices.UnitTest
{
  [TestClass]
  public class ModelDataProviderTests
  {
    public ModelDataProviderTests()
    {
      _connectionFactory = A.Fake<IConnectionFactory>();
      _modelDataProvider = new ModelDataProvider<TestDataModel>(_connectionFactory);
    }

    [TestMethod]
    public void ByKey_throws_exception_when_no_key_found_in_definition()
    {
      Action action = () => new ModelDataProvider<TestDataModelNoKey>(_connectionFactory).ByKey(5);

      action.MustThrow<InvalidOperationException>();
    }

    [TestMethod]
    public void Exists_throws_exception_when_no_key_found_in_definition()
    {
      Action action = () => new ModelDataProvider<TestDataModelNoKey>(_connectionFactory).Exists(5);

      action.MustThrow<InvalidOperationException>();
    }

    public class TestDataModel : DataModel
    {
      [Key]
      public int Id { get; set; }

      // editable column
      public string Title { get; set; }
    }

    public class TestDataModelNoKey : DataModel
    {
      public int Id { get; set; }

      // editable column
      public string Title { get; set; }
    }

    private readonly IConnectionFactory _connectionFactory;

    private readonly ModelDataProvider<TestDataModel> _modelDataProvider;
  }
}

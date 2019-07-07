using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SqlBuilder.DataServices.UnitTest
{
  [TestClass]
  public class DataPageTests
  {
    [TestMethod]
    public void count_uses_passed_in_value()
    {
      new DataPage<object>(null, 1234).Count.MustBe(1234);
    }

    [TestMethod]
    public void count_uses_data_count_when_count_not_passed_in()
    {
      // set-up
      IEnumerable<object> data = new object[]
      {
        new object(),
        new object(),
      };

      // assert
      new DataPage<object>(data).Count.MustBe(2);
    }

    [TestMethod]
    public void count_uses_passed_in_value_when_count_passed_in()
    {
      // set-up
      IEnumerable<object> data = new object[]
      {
        new object(),
        new object(),
      };

      // assert
      new DataPage<object>(data, 2344).Count.MustBe(2344);
    }
  }
}

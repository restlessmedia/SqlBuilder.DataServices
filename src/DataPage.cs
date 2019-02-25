using System.Collections.Generic;
using System.Linq;

namespace SqlBuilder.DataServices
{
  public struct DataPage<T>
  {
    public DataPage(IEnumerable<T> data, int? count = null)
    {
      Data = data;
      Count = count ?? data.Count();
    }

    public readonly IEnumerable<T> Data;

    public readonly int Count;
  }
}
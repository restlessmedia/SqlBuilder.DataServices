using System.Collections.Generic;
using System.Linq;

namespace SqlBuilder.DataServices
{
  public struct DataPage<T>
  {
    public DataPage(IEnumerable<T> data, int? count = null)
    {
      Data = data;
      _count = count;
    }

    public readonly IEnumerable<T> Data;

    public int Count
    {
      get
      {
        if (!_count.HasValue && Data != null)
        {
          _count = Data.Count();
        }

        return _count.GetValueOrDefault();
      }
    }

    private int? _count;
  }
}
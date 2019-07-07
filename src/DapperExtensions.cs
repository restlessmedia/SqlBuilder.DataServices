using SqlBuilder;
using System.Collections.Generic;

namespace Dapper
{
  internal static class DapperExtensions
  {
    public static object ToParameters(this object param)
    {
      if (param == null)
      {
        return param;
      }

      if (param is SqlMapper.IDynamicParameters)
      {
        return (SqlMapper.IDynamicParameters)param;
      }

      if (param is ParameterCollection)
      {
        return ToParameters(param as ParameterCollection);
      }

      return new DynamicParameters(param);
    }

    public static DynamicParameters ToParameters(ParameterCollection parameterCollection)
    {
      if (parameterCollection == null || parameterCollection.Count == 0)
      {
        return null;
      }

      DynamicParameters parameters = new DynamicParameters();

      foreach (KeyValuePair<string, object> pair in parameterCollection)
      {
        parameters.Add(pair.Key, pair.Value);
      }

      return parameters;
    }
  }
}
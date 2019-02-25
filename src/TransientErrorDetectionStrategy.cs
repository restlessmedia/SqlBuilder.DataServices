using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace SqlBuilder.DataServices
{
  public class TransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
  {
    public bool IsTransient(Exception ex)
    {
      if (ex is TimeoutException)
      {
        return true;
      }

      SqlException sqlException = ex as SqlException;

      if (sqlException != null)
      {
        return IsTransient(sqlException);
      }

      return false;
    }

    private static bool IsTransient(SqlException ex)
    {
      return ex.Errors.OfType<SqlError>().Any(IsTransient);
    }

    private static bool IsTransient(SqlError error)
    {
      return new[] { (int)SqlErrorCode.Deadlock, (int)SqlErrorCode.TimeoutExpired }.Contains(error.Number);
    }
  }
}
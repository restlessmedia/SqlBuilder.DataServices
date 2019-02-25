using System;
using System.Data;

namespace SqlBuilder.DataServices
{
  public interface ISqlExecute
  {
    int Execute(IDbTransaction transaction, string command, object param = null, CommandType commandType = CommandType.StoredProcedure);

    int Execute(string command, object param = null, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null);

    void Execute(Action<IDbConnection> action);

    int ExecuteWithTransaction(string command, object param = null, CommandType commandType = CommandType.StoredProcedure);

    void ExecuteWithTransaction(Action<IDbTransaction> action);
  }
}
using System.Data;

namespace SqlBuilder.DataServices
{
  public interface IConnectionFactory
  {
    IDbConnection CreateConnection(bool open = false);

    IDbTransaction CreateTransaction(IDbConnection connection);

    IDbTransaction CreateTransaction(bool open = false);
  }
}
using System.Data;

namespace SqlBuilder.DataServices
{
  public interface IConnectionFactory
  {
    IDbConnection CreateConnection(bool open = true);

    IDbTransaction CreateTransaction(IDbConnection connection);

    IDbTransaction CreateTransaction(bool open = true);
  }
}
using System;
using System.Data;
using System.Data.SqlClient;

namespace SqlBuilder.DataServices
{
  public class ConnectionFactory : IConnectionFactory
  {
    public ConnectionFactory(string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString))
      {
        throw new ArgumentNullException(nameof(connectionString));
      }

      _connectionString = connectionString;
    }

    public virtual IDbConnection CreateConnection(bool open = false)
    {
      try
      {
        return new SqlConnection(_connectionString);
      }
      catch (Exception e)
      {
        throw new Exception($"An exception occured when creating a connection, is the connection string configured correctly? Original error: {e.Message}");
      }
    }

    public virtual IDbTransaction CreateTransaction(bool open = false)
    {
      return CreateTransaction(CreateConnection(open));
    }

    public virtual IDbTransaction CreateTransaction(IDbConnection connection)
    {
      return connection.BeginTransaction();
    }

    private readonly string _connectionString;
  }
}
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

    public ConnectionFactory(IDbConnection connection)
    {
      _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public virtual IDbConnection CreateConnection(bool open = false)
    {
      try
      {
        IDbConnection connection;

        if (_connection != null)
        {
          connection = _connection;
        }
        else
        {
          connection = new SqlConnection(_connectionString);
        }

        if (open && connection.State == ConnectionState.Closed)
        {
          connection.Open();
        }

        return connection;
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

    private readonly IDbConnection _connection;
  }
}
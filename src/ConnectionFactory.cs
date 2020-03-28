using System;
using System.Data;
using System.Data.SqlClient;

namespace SqlBuilder.DataServices
{
  public class ConnectionFactory : IConnectionFactory
  {
    public ConnectionFactory(Func<IDbConnection> connectionFactory)
    {
      _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public ConnectionFactory(string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString))
      {
        throw new ArgumentNullException(nameof(connectionString));
      }

      _connectionFactory = () => new SqlConnection(connectionString);
    }

    public virtual IDbConnection CreateConnection(bool open = false)
    {
      try
      {
        IDbConnection connection = _connectionFactory();

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

    private readonly Func<IDbConnection> _connectionFactory;
  }
}
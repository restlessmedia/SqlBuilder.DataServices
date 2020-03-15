using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Xunit;

namespace SqlBuilder.DataServices.IntegrationTest
{
  /// <summary>
  /// Uses sql lite to perform tests.
  /// </summary>
  public class GridReaderWrapperTests
  {
    /// <summary>
    /// This is the only way I could add a test for the issue where a proc would early return and not output any recordsets.
    /// Dapper treats this with a <see cref="InvalidOperationException"/> but really, we just want to return no records.
    /// </summary>
    [Fact]
    public void Read_does_not_throw_when_empty_recordset_returned()
    {
      using (GridReaderWrapper gridReaderWrapper = CreateInstance(string.Empty))
      {
        IEnumerable<dynamic> data = gridReaderWrapper.Read().ToList();
      }
    }

    private GridReaderWrapper CreateInstance(string sql)
    {
      IDbConnection connection = CreateConnection();
      return new GridReaderWrapper(connection, sql, null, commandType: CommandType.Text);
    }

    private IDbConnection CreateConnection()
    {
      SQLiteConnectionStringBuilder connectionStringBuilder = new SQLiteConnectionStringBuilder { DataSource = ":memory:" };
      string connectionString = connectionStringBuilder.ToString();
      SQLiteConnection connection = new SQLiteConnection(connectionString);
      return connection;
    }
  }
}
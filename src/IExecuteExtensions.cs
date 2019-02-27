using System.Data;

namespace SqlBuilder.DataServices
{
  public static class IExecuteExtensions
  {
    /// <summary>
    /// Executes the <see cref="SqlText"/>.
    /// </summary>
    /// <param name="sqlExecute"></param>
    /// <param name="sqlText"></param>
    /// <returns></returns>
    public static int Execute(this ISqlExecute sqlExecute, SqlText sqlText)
    {
      string sql = sqlText.Sql();

      if (string.IsNullOrEmpty(sql))
      {
        return 0;
      }

      return sqlExecute.Execute(sql, sqlText.Parameters, CommandType.Text);
    }
  }
}
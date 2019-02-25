namespace SqlBuilder.DataServices
{
  public static class IExecuteExtensions
  {
    public static int Execute(this ISqlExecute sqlExecute, SqlText sqlText)
    {
      string sql = sqlText.Sql();

      if (string.IsNullOrEmpty(sql))
      {
        return 0;
      }

      return sqlExecute.Execute(sql, sqlText.Parameters);
    }
  }
}
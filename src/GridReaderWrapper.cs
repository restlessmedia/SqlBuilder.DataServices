using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SqlBuilder.DataServices
{
  internal class GridReaderWrapper : IGridReader
  {
    public GridReaderWrapper(IDbConnection connection, string command, object param, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null)
    {
      // changed this so we can pass in empty string when testing empty rows issue and invalid operation exception
      if (command == null)
      {
        throw new ArgumentNullException(nameof(command));
      }

      _connection = connection ?? throw new ArgumentNullException(nameof(connection));
      _reader = _connection.QueryMultiple(command, new DynamicParameters(param), commandTimeout: commandTimeout, commandType: commandType);
    }

    public IEnumerable<T> Read<T>()
    {
      return _reader.Read<T>();
    }

    public IEnumerable<dynamic> Read()
    {
      try
      {
        return _reader.Read();
      }
      catch (InvalidOperationException e)
      {
        if (IsDapperEmptyColumnException(e))
        {
          return Enumerable.Empty<dynamic>();
        }

        throw e;
      }
    }

    public IEnumerable<TReturn> Read<TFirst, TSecond, TReturn>(Func<TFirst, TSecond, TReturn> func, string splitOn = DefaultSplitOn)
    {
      try
      {
        return _reader.Read(func, splitOn);
      }
      catch (InvalidOperationException e)
      {
        if (IsDapperEmptyColumnException(e))
        {
          return Enumerable.Empty<TReturn>();
        }

        throw e;
      }
    }

    public IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> func, string splitOn = DefaultSplitOn)
    {
      try
      {
        return _reader.Read(func, splitOn);
      }
      catch (InvalidOperationException e)
      {
        if (IsDapperEmptyColumnException(e))
        {
          return Enumerable.Empty<TReturn>();
        }

        throw e;
      }
    }

    public IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TReturn> func, string splitOn = DefaultSplitOn)
    {
      try
      {
        return _reader.Read(func, splitOn);
      }
      catch (InvalidOperationException e)
      {
        if (IsDapperEmptyColumnException(e))
        {
          return Enumerable.Empty<TReturn>();
        }

        throw e;
      }
    }

    public IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> func, string splitOn = DefaultSplitOn)
    {
      try
      {
        return _reader.Read(func, splitOn);
      }
      catch (InvalidOperationException e)
      {
        if (IsDapperEmptyColumnException(e))
        {
          return Enumerable.Empty<TReturn>();
        }

        throw e;
      }
    }

    public IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> func, string splitOn = DefaultSplitOn)
    {
      try
      {
        return _reader.Read(func, splitOn);
      }
      catch (InvalidOperationException e)
      {
        if (IsDapperEmptyColumnException(e))
        {
          return Enumerable.Empty<TReturn>();
        }

        throw e;
      }
    }

    public IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> func, string splitOn = DefaultSplitOn)
    {
      try
      {
        return _reader.Read(func, splitOn);
      }
      catch (InvalidOperationException e)
      {
        if (IsDapperEmptyColumnException(e))
        {
          return Enumerable.Empty<TReturn>();
        }

        throw e;
      }
    }

    public const string DefaultSplitOn = "Id";

    public void Dispose()
    {
      if (_connection.State != ConnectionState.Closed)
      {
        _connection.Close();
      }

      _reader.Dispose();
    }

    private static bool IsDapperEmptyColumnException(InvalidOperationException exception)
    {
      return exception.Message == "No columns were selected";
    }

    private readonly SqlMapper.GridReader _reader;

    private readonly IDbConnection _connection;
  }
}
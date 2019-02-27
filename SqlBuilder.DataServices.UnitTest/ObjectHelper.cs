using System;
using System.Collections;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace SqlBuilder.DataServices.UnitTest
{
  internal class ObjectHelper
  {
    public static T Create<T>()
    {
      return (T)FormatterServices.GetUninitializedObject(typeof(T));
    }

    public static SqlException CreateSqlException(string message, int errorCode)
    {
      SqlException exception = Create<SqlException>();

      SetProperty(exception, "_message", message);

      var errors = new ArrayList();
      var errorCollection = Create<SqlErrorCollection>();

      SetProperty(errorCollection, "errors", errors);

      var error = Create<SqlError>();

      SetProperty(error, "number", errorCode);

      errors.Add(error);

      SetProperty(exception, "_errors", errorCollection);

      return exception;
    }

    private static void SetProperty<T>(T targetObject, string fieldName, object value)
    {
      var field = typeof(T).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
      if (field != null)
      {
        field.SetValue(targetObject, value);
      }
      else
      {
        throw new InvalidOperationException("No field with name " + fieldName);
      }
    }
  }

  internal class ObjectHelper<T>
    where T : class
  {
    private static MemberInfo GetMember<TProp>(Expression<Func<T, TProp>> expression)
    {
      MemberExpression memberExpression = expression.Body as MemberExpression;

      if (memberExpression == null)
      {
        throw new ArgumentException($"Expression '{expression}' refers to a method, not a property or field.");
      }

      MemberInfo member = memberExpression.Member as MemberInfo;

      if (member == null)
      {
        throw new ArgumentException($"Expression '{expression}' is not a member.");
      }

      return member;
    }
  }
}
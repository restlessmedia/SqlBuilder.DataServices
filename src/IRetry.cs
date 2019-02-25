using System;

namespace SqlBuilder.DataServices
{
  public interface IRetry
  {
    T Retry<T>(Func<T> action);

    void Retry(Action action);

    T Retry<T>(Func<T> action, int retryCount, TimeSpan interval);

    void Retry(Action action, int retryCount, TimeSpan interval);
  }
}
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;

namespace SqlBuilder.DataServices
{
  public class SqlRetry : IRetry
  {
    public T Retry<T>(Func<T> action)
    {
      return Retry(action, DefaultRetryCount, DefaultInterval);
    }

    public void Retry(Action action)
    {
      Retry(action, DefaultRetryCount, DefaultInterval);
    }

    public T Retry<T>(Func<T> action, int retryCount, TimeSpan interval)
    {
      return GetPolicy(retryCount, interval).ExecuteAction(action);
    }

    public void Retry(Action action, int retryCount, TimeSpan interval)
    {
      GetPolicy(retryCount, interval).ExecuteAction(action);
    }

    public const int DefaultRetryCount = 3;

    public static TimeSpan DefaultInterval = TimeSpan.FromSeconds(1);

    private static RetryPolicy<TransientErrorDetectionStrategy> GetPolicy(int retryCount, TimeSpan interval)
    {
      FixedInterval strategy = new FixedInterval(retryCount, interval);
      RetryPolicy<TransientErrorDetectionStrategy> policy = new RetryPolicy<TransientErrorDetectionStrategy>(strategy);
      return policy;
    }
  }
}
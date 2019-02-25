using System.ComponentModel;

namespace SqlBuilder.DataServices
{
  public enum SqlErrorCode
  {
    None = 0,
    [Description("The instance of SQL Server does not support encryption.")]
    NoEncryptionSupport = 20,
    [Description("An error occurred during login.")]
    LoginError = 64,
    [Description("Connection initialization error.")]
    ConnectionInitializationError = 233,
    [Description("A transport-level error occurred when receiving results from the server.")]
    TransportReceiveError = 10053,
    [Description("A transport-level error occurred when sending the request to the server.")]
    TransportSendError = 10054,
    [Description("Network or instance-specific error.")]
    NetworkError = 10060,
    [Description("Connection could not be initialized.")]
    ConnectionInitializationFailure = 40143,
    [Description("The service encountered an error processing your request.")]
    ErrorProcessingRequest = 40197,
    [Description("The server is busy")]
    ServerBusy = 40501,
    [Description("The database is currently unavailable.")]
    DatabaseUnavailable = 40613,
    [Description("Deadlock.")]
    Deadlock = 1205,
    [Description("Timeout expired.")]
    TimeoutExpired = -2
  }
}
namespace BPN.PaymentGateway.Infrastructure.Logging;

/// <summary>
/// Defined log types
/// </summary>
public enum LogType
{
    Exception = 0,
    HttpException,
    HttpRequestResponse,
    Database,
    Eventbus,
    Scheduler,
    Processor,
    Default,
    Audit,
    Info,
    BackgroundJob,
    BackgroundJobException,
    ExternalServiceCall,
    ExternalServiceException,
    Cache,
    DbConnectionOpen,
    InternalServiceCall,
    DatabaseError,
    Weebhook
}
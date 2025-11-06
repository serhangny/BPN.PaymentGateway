namespace BPN.PaymentGateway.Application.Common.Models;

/// <summary>
/// Represents the standard base response structure for all API or service operations.
/// It contains the success status and any error messages if the operation failed.
/// </summary>
public class BaseResponse
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; }
    
    /// <summary>
    /// Gets the list of error messages associated with the response.
    /// This array is empty for successful responses.
    /// </summary>
    public string[] Errors { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseResponse"/> class.
    /// </summary>
    /// <param name="isSuccess">The status indicating whether the operation succeeded.</param>
    /// <param name="errors">The array of error messages.</param>
    protected BaseResponse(bool isSuccess, string[] errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    /// <summary>
    /// Creates a successful <see cref="BaseResponse"/> instance with no data or errors.
    /// </summary>
    /// <returns>A successful BaseResponse object.</returns>
    public static BaseResponse Success() => new(true, Array.Empty<string>());
    
    /// <summary>
    /// Creates a failed <see cref="BaseResponse"/> instance with the specified error messages.
    /// </summary>
    /// <param name="errors">The error messages to be included in the response.</param>
    /// <returns>A failed BaseResponse object.</returns>
    public static BaseResponse Failure(params string[] errors) => new(false, errors);
}

/// <summary>
/// Represents a generic base response structure that can carry data of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of data the response will carry.</typeparam>
public class BaseResponse<T> : BaseResponse
{
    /// <summary>
    /// Gets the data returned by the successful operation. This will be null (or default) for failed responses.
    /// </summary>
    public T? Data { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseResponse{T}"/> class.
    /// </summary>
    /// <param name="isSuccess">The status indicating whether the operation succeeded.</param>
    /// <param name="data">The data payload.</param>
    /// <param name="errors">The array of error messages.</param>
    private BaseResponse(bool isSuccess, T? data, string[] errors) : base(isSuccess, errors)
    {
        Data = data;
    }

    /// <summary>
    /// Creates a successful <see cref="BaseResponse{T}"/> instance containing the specified data.
    /// </summary>
    /// <param name="data">The data to be returned in the response.</param>
    /// <returns>A successful BaseResponse&lt;T&gt; object with data.</returns>
    public static BaseResponse<T> Success(T data) => new(true, data, []);
    
    /// <summary>
    /// Creates a failed <see cref="BaseResponse{T}"/> instance with the specified error messages. The Data field will be the default value for <typeparamref name="T"/>.
    /// </summary>
    /// <param name="errors">The error messages to be included in the response.</param>
    /// <returns>A failed BaseResponse&lt;T&gt; object.</returns>
    public new static BaseResponse<T> Failure(params string[] errors) => new(false, default, errors);
}
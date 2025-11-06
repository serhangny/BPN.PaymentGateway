using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

using BPN.PaymentGateway.Application.Orders.Commands;
using BPN.PaymentGateway.Application.Orders.Models;
using BPN.PaymentGateway.Application.Products.Models;
using BPN.PaymentGateway.Application.Balances.Models;

namespace BPN.PaymentGateway.Application.Clients;

/// <summary>
/// Implementation of balance management client
/// </summary>
public class BalanceManagementClient : IBalanceManagementClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BalanceManagementClient> _logger;

    /// <summary>
    /// CTOR
    /// </summary>
    public BalanceManagementClient(HttpClient httpClient, ILogger<BalanceManagementClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Gets products from balance management
    /// </summary>
    public async Task<ProductListResponse?> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("api/products", cancellationToken);
       
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogDebug("Products API response: {Response}", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Failed to fetch products. Status: {response.StatusCode}, Body: {content}");
        }

        if (string.IsNullOrWhiteSpace(content))
            return null;

        return JsonSerializer.Deserialize<ProductListResponse>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    /// <summary>
    /// Gets user balance async
    /// </summary>
    public async Task<BalanceResponse?> GetBalanceAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("api/balance", cancellationToken);
       
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogDebug("Balance API response: {Response}", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Failed to fetch balance. Status: {response.StatusCode}, Body: {content}");
        }

        if (string.IsNullOrWhiteSpace(content))
            return null;

        return JsonSerializer.Deserialize<BalanceResponse>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    /// <summary>
    /// Creates a pre order
    /// </summary>
    public async Task<PreOrderResponse?> CreatePreorderAsync(CreateOrderCommand preOrder, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync("api/balance/preorder", preOrder, cancellationToken);

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        _logger.LogDebug("Preorder API raw response: {Content}", content);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Preorder failed: {Status} {Body}", response.StatusCode, content);
            throw new HttpRequestException($"Balance Management returned {response.StatusCode}");
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            _logger.LogWarning("Preorder response body was empty");
            return null;
        }

        try
        {
            var result = JsonSerializer.Deserialize<PreOrderResponse>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result is null)
                throw new InvalidOperationException("Preorder deserialization returned null");

            return result;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize preorder response: {Body}", content);
            throw new InvalidOperationException("Invalid JSON response from Balance Management service");
        }
    }

    /// <summary>
    /// Completes response
    /// </summary>
    public async Task<CompleteOrderResponse?> CompleteOrderAsync(string orderId, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync("api/balance/complete", orderId, cancellationToken);

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        _logger.LogDebug("Complete order API raw response: {Content}", content);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Preorder failed: {Status} {Body}", response.StatusCode, content);
            throw new HttpRequestException($"Balance Management returned {response.StatusCode}");
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            _logger.LogWarning("Preorder response body was empty");
            return null;
        }

        try
        {
            var result = JsonSerializer.Deserialize<CompleteOrderResponse>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result is null)
                throw new InvalidOperationException("Complete order deserialization returned null");

            return result;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize complete order response: {Body}", content);
            throw new InvalidOperationException("Invalid JSON response from Balance Management service");
        }
    }
}
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using BPN.PaymentGateway.Application.Orders.Commands;
using BPN.PaymentGateway.Application.Orders.Models;
using BPN.PaymentGateway.Application.Products.Models;

namespace BPN.PaymentGateway.Application.Clients;

/// <summary>
/// Implementation of balance management client
/// </summary>
public class BalanceManagementClient : IBalanceManagementClient
{
    private const string CacheKey = "balanceManagementClient";
    
    private readonly HttpClient _httpClient;
    private readonly ILogger<BalanceManagementClient> _logger;
    //private readonly IMemoryCache _memoryCache;

    /// <summary>
    /// CTOR
    /// </summary>
    public BalanceManagementClient(HttpClient httpClient, ILogger<BalanceManagementClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        //_memoryCache = memoryCache;
    }

    /// <summary>
    /// Gets products from balance management
    /// </summary>
    public async Task<ProductListResponse?> GetProductsAsync()
    {
        var response = await _httpClient.GetAsync("api/products");
       
        var content = await response.Content.ReadAsStringAsync();
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
    /// Creates a pre order
    /// </summary>
    public async Task<PreOrderResponse?> CreatePreorderAsync(CreateOrderCommand preOrder)
    {
        using var response = await _httpClient.PostAsJsonAsync("api/preorder", preOrder);

        var content = await response.Content.ReadAsStringAsync();

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
}
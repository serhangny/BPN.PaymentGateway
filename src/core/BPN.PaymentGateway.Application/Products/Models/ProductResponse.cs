using System.Text.Json.Serialization;

namespace BPN.PaymentGateway.Application.Products.Models;

/// <summary>
/// Represents the detailed information for an e-commerce product.
/// It maps the camelCase fields from the JSON to PascalCase properties in C#.
/// </summary>
public class ProductResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The price of the product. Using decimal is a best practice for monetary values.
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("stock")]
    public int Stock { get; set; }
}

/// <summary>
/// Represents the overall API response structure for a list of products.
/// It includes the success status and the list of products (Data).
/// </summary>
public class ProductListResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the list of products, mapping to the 'data' array in the JSON payload.
    /// </summary>
    [JsonPropertyName("data")]
    public List<ProductResponse> Data { get; set; } = new List<ProductResponse>();
}
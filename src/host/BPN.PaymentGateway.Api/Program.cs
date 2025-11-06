using System.IO.Compression;
using System.Net.Http.Headers;
using Asp.Versioning.ApiExplorer;

using Microsoft.AspNetCore.ResponseCompression;

using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using FluentValidation.AspNetCore;

using BPN.PaymentGateway.Api.Extensions;
using BPN.PaymentGateway.Api.Filters;
using BPN.PaymentGateway.Application.Clients;
using BPN.PaymentGateway.Application.Extensions;
using BPN.PaymentGateway.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Use host logging(Could be serilog)
//builder.Host.UseLogging(); 

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add Routing
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add API Versioning
builder.Services.AddApiVersioningWithVersionReader();

// Add application layer
builder.Services.AddApplication(builder.Configuration);

// Add infra layer
builder.Services.AddInfrastructure(builder.Configuration);

// Add Middlewares
builder.Services.AddMiddlewares();

# region ------------------- HTTP CLIENT CONFIGURATION -------------------

var balanceApiBaseUrl = builder.Configuration["BalanceManagementApi:BaseUrl"];

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
    HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => 
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
    HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

builder.Services.AddHttpClient<IBalanceManagementClient, BalanceManagementClient>(client =>
    {
        client.BaseAddress = new Uri(balanceApiBaseUrl);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        client.Timeout = TimeSpan.FromSeconds(10);
    })
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());

#endregion

// Add services to the container.
builder.Services.AddControllers().AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<Program>();
});



builder.Services.AddSwaggerGen(c =>
{
    var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider()
        .GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        c.SwaggerDoc(description.GroupName, new OpenApiInfo
        {
            Title = "BPN Payment Gateway API",
            Version = description.ApiVersion.ToString(),
            Description = "A Clean Architecture API"
        });
    }

    c.OperationFilter<ApiVersionOperationFilter>();
});

builder.Services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });
builder.Services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseInfrastructure();

app.MapControllers();

app.Run();

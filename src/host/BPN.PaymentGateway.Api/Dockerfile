FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV LANG=tr_TR.UTF-8
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/host/BPN.PaymentGateway.Api/BPN.PaymentGateway.Api.csproj", "src/host/BPN.PaymentGateway.Api/"]
COPY ["src/core/BPN.PaymentGateway.Application/BPN.PaymentGateway.Application.csproj", "src/core/BPN.PaymentGateway.Application/"]
COPY ["src/core/BPN.PaymentGateway.Domain/BPN.PaymentGateway.Domain.csproj", "src/core/BPN.PaymentGateway.Domain/"]
COPY ["src/infrastructure/BPN.PaymentGateway.Infrastructure/BPN.PaymentGateway.Infrastructure.csproj", "src/infrastructure/BPN.PaymentGateway.Infrastructure/"]
RUN dotnet restore "src/host/BPN.PaymentGateway.Api/BPN.PaymentGateway.Api.csproj"
COPY . .
WORKDIR "/src/src/host/BPN.PaymentGateway.Api"
RUN dotnet build "BPN.PaymentGateway.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BPN.PaymentGateway.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BPN.PaymentGateway.Api.dll"]

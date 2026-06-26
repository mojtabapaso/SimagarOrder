FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["SimagarOrder.API/SimagarOrder.API.csproj", "SimagarOrder.API/"]
COPY ["SimagarOrder.Application/SimagarOrder.Application.csproj", "SimagarOrder.Application/"]
COPY ["SimagarOrder.Domain/SimagarOrder.Domain.csproj", "SimagarOrder.Domain/"]
COPY ["SimagarOrder.Infrastructure/SimagarOrder.Infrastructure.csproj", "SimagarOrder.Infrastructure/"]


RUN dotnet restore "SimagarOrder.API/SimagarOrder.API.csproj"

COPY . .

WORKDIR /src/SimagarOrder.API

RUN dotnet build "SimagarOrder.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish

RUN dotnet publish "SimagarOrder.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish   /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimagarOrder.API.dll"]

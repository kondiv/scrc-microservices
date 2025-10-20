FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["services/EquipmentService/EquipmentService.Api/EquipmentService.Api.csproj", "services/EquipmentService/EquipmentService.Api/"]
COPY ["services/EquipmentService/EquipmentService.Domain/EquipmentService.Domain.csproj", "services/EquipmentService/EquipmentService.Domain/"]
COPY ["services/EquipmentService/EquipmentService.Infrastructure/EquipmentService.Infrastructure.csproj", "services/EquipmentService/EquipmentService.Infrastructure/"]

COPY ["shared/Shared/Shared.ResultPattern/Shared.ResultPattern.csproj", "shared/Shared/Shared.ResultPattern/"]

RUN dotnet restore "services/EquipmentService/EquipmentService.Api/EquipmentService.Api.csproj"
COPY . .
WORKDIR "/src/services/EquipmentService/EquipmentService.Api/"
RUN dotnet build "./EquipmentService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EquipmentService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EquipmentService.Api.dll"]

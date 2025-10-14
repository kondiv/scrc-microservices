FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["services/AuthenticationService/Api/Api.csproj", "services/AuthenticationService/Api/"]
COPY ["services/AuthenticationService/Common/Common.csproj", "services/AuthenticationService/Common/"]
COPY ["services/AuthenticationService/Domain/Domain.csproj", "services/AuthenticationService/Domain/"]
COPY ["services/AuthenticationService/Infrastructure/Infrastructure.csproj", "services/AuthenticationService/Infrastructure/"]

COPY ["shared/Shared/Shared.Auth/Shared.Auth.csproj", "shared/Shared/Shared.Auth/"]
COPY ["shared/Shared/Shared.ResultPattern/Shared.ResultPattern.csproj", "shared/Shared/Shared.ResultPattern/"]

RUN dotnet restore "services/AuthenticationService/Api/Api.csproj"
COPY . .
WORKDIR "/src/services/AuthenticationService/Api"
RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]

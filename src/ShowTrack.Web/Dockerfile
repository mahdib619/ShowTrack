FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ShowTrack.Web/ShowTrack.Web.csproj", "src/ShowTrack.Web/"]
COPY ["src/Migrators/PostgreSQLMigrator/PostgresqlMigrator.csproj", "src/Migrators/PostgreSQLMigrator/"]
COPY ["src/ShowTrack.Data/ShowTrack.Data.csproj", "src/ShowTrack.Data/"]
COPY ["src/ShowTrack.Domain/ShowTrack.Domain.csproj", "src/ShowTrack.Domain/"]
COPY ["src/Migrators/SqlServerMigrator/SqlServerMigrator.csproj", "src/Migrators/SqlServerMigrator/"]
COPY ["src/ShowTrack.Contracts/ShowTrack.Contracts.csproj", "src/ShowTrack.Contracts/"]
RUN dotnet restore "./src/ShowTrack.Web/ShowTrack.Web.csproj"
COPY . .
WORKDIR "/src/src/ShowTrack.Web"
RUN dotnet build "./ShowTrack.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ShowTrack.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShowTrack.Web.dll"]
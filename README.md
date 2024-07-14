# ShowTrack

This application is designed to help users track their favorite shows, schedule upcoming seasons and receive notifications for show timings.

## Technologies Used

- **ASP.NET Core Web API**: Backend API services.
- **Blazor WebAssembly (WASM)**: Frontend user interface.
- **EF Core**: Database interactions.
- **SQL Server, PostgreSQL and In-Memory Database**: Supported database providers.
- **ASP.NET Identity**: User authentication and management.

## Database Configuration

The application supports three database providers: SQL Server, PostgreSQL and In-Memory database. You can configure the database provider in the [appsettings.json](./src/ShowTrack.Web/appsettings.json) file under the `DbProvider` section.

### SQL Server

- Provider Name: `mssql`
- Example configuration:

  ```json
  {
    "DbProvider": "mssql",
    "ConnectionStrings": {
      "DefaultConnection": "Server=your_server;Database=ShowTrackDb;User Id=your_user;Password=your_password;"
    }
  }
  ```

### PostgreSQL

- Provider Name: `npgsql`
- Example configuration:

  ```json
  {
    "DbProvider": "npgsql",
    "ConnectionStrings": {
      "DefaultConnection": "Host=your_host;Database=ShowTrackDb;Username=your_user;Password=your_password"
    }
  }
  ```

### In-Memory Database

- Provider Name: `memory`
- Example configuration:

  ```json
  {
    "DbProvider": "memory"
  }
  ```

  **Database Migration**
  - Ensure you are in the [`src/ShowTrack.Web`](./src/ShowTrack.Web) directory.
  - Apply migrations to set up the database (based on proivder).

     **SQL Server**

     ```bash
     dotnet ef database update -p ../Migrators/SqlServerMigrator/SqlServerMigrator.csproj  -- --dbprovider mssql
     ```

     **PostgreSQL**

     ```bash
     dotnet ef database update -p ../Migrators/PostgresqlMigrator/PostgresqlMigrator.csproj  -- --dbprovider npgsql
     ```

## Admin Configuration

Configure admin username and password in [appsettings.json](./src/ShowTrack.Web/appsettings.json), an admin user will be created automatically.

```json
{
  "AdminUserName": "",
  "AdminPassword": ""
}
```

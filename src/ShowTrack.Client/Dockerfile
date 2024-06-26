# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the csproj and restore as distinct layers
COPY ["src/ShowTrack.Client/ShowTrack.Client.csproj", "src/ShowTrack.Client/"]
COPY ["src/ShowTrack.Domain/ShowTrack.Domain.csproj", "src/ShowTrack.Domain/"]
COPY ["src/ShowTrack.Contracts/ShowTrack.Contracts.csproj", "src/ShowTrack.Contracts/"]

# Copy everything else and build
RUN dotnet restore "./src/ShowTrack.Client/ShowTrack.Client.csproj"
COPY . .
WORKDIR "/src/src/ShowTrack.Client"
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish

# Stage 2: Serve
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# Install envsubst and jq for environment variable substitution and JSON manipulation
RUN apk add --no-cache gettext jq

# Remove the default Nginx static files
RUN rm -rf ./*

# Copy the published Blazor WebAssembly project from the build stage
COPY --from=build /app/publish/wwwroot .

# Copy custom Nginx configuration template file
COPY --from=build /app/publish/nginx.conf.template /etc/nginx/nginx.conf.template

# Copy the entrypint file
COPY --from=build /app/publish/entrypoint.sh /entrypoint.sh

# Make the update script executable
RUN chmod +x /entrypoint.sh

# Expose the port that Nginx is running on (default to 80)
EXPOSE 80

# Set environment variables for the port and server name
ENV PORT=80
ENV SERVER_NAME=0.0.0.0
ENV BASE_URL=https://localhost:8000/

ENTRYPOINT ["/entrypoint.sh"]
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/ShowTrack.Client/ShowTrack.Client.csproj", "src/ShowTrack.Client/"]
COPY ["src/ShowTrack.Domain/ShowTrack.Domain.csproj", "src/ShowTrack.Domain/"]
COPY ["src/ShowTrack.Contracts/ShowTrack.Contracts.csproj", "src/ShowTrack.Contracts/"]

RUN dotnet restore "./src/ShowTrack.Client/ShowTrack.Client.csproj"
COPY . .
WORKDIR "/src/src/ShowTrack.Client"
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish

FROM nginx:alpine
WORKDIR /usr/share/nginx/html

RUN rm -rf ./*

COPY --from=build /app/publish/wwwroot .

COPY --from=build /app/publish/nginx.conf.template /etc/nginx/nginx.conf.template

EXPOSE 80

ENV PORT=80
ENV SERVER_NAME=0.0.0.0

ENTRYPOINT ["nginx", "-g", "daemon off;"]
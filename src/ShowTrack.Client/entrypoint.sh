#!/bin/sh

# Check if BASE_URL environment variable is set
if [ -z "$BASE_URL" ]; then
  echo "BASE_URL is not set. Using default value."
else
  echo "Updating BaseUrl to $BASE_URL in appsettings.json"

  # Update the BaseUrl property in appsettings.json
  jq --arg baseUrl "$BASE_URL" '.BaseAddress = $baseUrl' /app/wwwroot/appsettings.json > /app/wwwroot/appsettings.tmp.json && mv /app/wwwroot/appsettings.tmp.json /usr/share/nginx/html/appsettings.json
fi

# Generate nginx conf file from template
rm /etc/nginx/conf.d/*.conf
export uri="\$uri" #this variable used by nginx and should no be replaced by envsubst
envsubst < /etc/nginx/nginx.conf.template > /etc/nginx/conf.d/default.conf

nginx -g "daemon off;"
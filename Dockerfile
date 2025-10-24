FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

RUN apt-get update && apt-get install -y netcat-openbsd && rm -rf /var/lib/apt/lists/*

USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["InventorySystem-webapi.csproj", "."]
RUN dotnet restore "./InventorySystem-webapi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./InventorySystem-webapi.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InventorySystem-webapi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

COPY entrypoint.sh .

RUN sed -i 's/\r$//' entrypoint.sh && chmod +x /app/entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]

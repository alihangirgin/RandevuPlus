# Base Image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 7159

# Build Image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["RandevuPlus.sln", "./"]
COPY ["RandevuPlus.API/RandevuPlus.API.csproj", "RandevuPlus.API/"]
RUN dotnet restore
COPY . .
WORKDIR "/src/RandevuPlus.API"
RUN dotnet build -c Release -o /app/build

# Publish Image
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Final Image
FROM base AS final
WORKDIR /app

#COPY ["RandevuPlus.API/certs/localhost.pfx", "/app/certs/localhost.pfx"]

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RandevuPlus.API.dll"]

# Ek izinler
USER root
RUN mkdir -p /app/wwwroot/uploads && chmod -R 777 /app/wwwroot/uploads
USER 1000:1000

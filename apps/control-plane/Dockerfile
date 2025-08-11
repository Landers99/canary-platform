# apps/control-plane/Dockerfile

# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj & restore first (better layer caching)
COPY ControlPlane.csproj ./
RUN dotnet restore

# Copy the rest and publish
COPY . ./
RUN dotnet publish -c Release -o /app/out /p:UseAppHost=false

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/out ./

# Kestrel will bind to ASPNETCORE_URLS provided by compose (8080)
ENTRYPOINT ["dotnet", "ControlPlane.dll"]

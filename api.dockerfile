# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
LABEL author="Beata Muszynska"
WORKDIR /app

# Zaktualizuj listę pakietów i zainstaluj potrzebne narzędzia
RUN apt-get update && apt-get install -y sqlite3
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
ENV PORT=7217
EXPOSE $PORT

# Copy csproj files and restore dependencies
COPY *.sln .
COPY API/API.csproj ./API/
COPY Core/Core.csproj ./Core/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY Infrastructure/Data/SeedData/ ./Infrastructure/Data/SeedData/

# Copy SSL certificates and database file
COPY API/certificate.pfx /app/ssl/
COPY API/tracker.db /app/

RUN dotnet restore

# Copy the entire project and build
COPY . .
RUN dotnet build -c Release --no-restore

# Run database migrations (uncomment if needed)
#RUN dotnet ef database drop -p Infrastructure -s API
#RUN dotnet ef migrations remove -p Infrastructure -s API || true
#RUN dotnet ef migrations add InitialCreate -p Infrastructure -s API -o Data/Migrations
#RUN dotnet ef database update --project API/API.csproj

RUN apt-get update
RUN apt-get install -y curl

# Publish the application
FROM build AS publish
RUN dotnet publish API/API.csproj -c Release -o /app/publish

# Stage 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files
COPY --from=publish /app/publish .

# Copy SSL certificates and database file to the final image
COPY --from=build /app/ssl /app/ssl
COPY --from=build /app/Infrastructure/Data/SeedData/ /app/Infrastructure/Data/SeedData/
COPY --from=build /app/tracker.db /app/tracker.db

# Set environment variable for HTTPS
ENV ASPNETCORE_URLS=https://+:7217

ENTRYPOINT ["dotnet", "API.dll"]

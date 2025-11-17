# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files trước để cache restore
COPY ["StudentActivies/StudentActivities.csproj", "StudentActivies/"]
COPY ["StudentActivities.Tests/StudentActivities.Tests.csproj", "StudentActivities.Tests/"]

# Restore dependencies
RUN dotnet restore "StudentActivies/StudentActivities.csproj"

# Copy toàn bộ source
COPY . .

# Build solution (không publish)
RUN dotnet build "StudentActivies/StudentActivities.csproj" -c Release --no-restore

# Run unit tests
RUN dotnet test "StudentActivities.Tests/StudentActivities.Tests.csproj" --no-build --logger:trx

# Stage 2: Publish
FROM build AS publish
WORKDIR /src/StudentActivies
RUN dotnet publish "StudentActivities.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Expose API ports
EXPOSE 8080
EXPOSE 8081

# Copy publish output
COPY --from=publish /app/publish .

# Entry point để chạy ứng dụng khi container khởi động
ENTRYPOINT ["dotnet", "StudentActivities.dll"]
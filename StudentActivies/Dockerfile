# Stage 1: Xây dựng ứng dụng
# Sử dụng .NET 8 SDK image để build dự án.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sao chép file .csproj và restore dependencies trước.
# Điều này tận dụng Docker layer caching, vì dependencies ít khi thay đổi.
COPY ["StudentActivities.csproj", "."]
RUN dotnet restore "./StudentActivities.csproj"

# Sao chép toàn bộ mã nguồn còn lại của dự án.
COPY . .
WORKDIR "/src/."

# Build và publish ứng dụng với configuration là Release.
# Kết quả publish sẽ được đưa vào thư mục /app/publish.
RUN dotnet publish "StudentActivities.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Create the final runtime image
# Sử dụng ASP.NET 8 runtime image, nhẹ hơn rất nhiều so với SDK image.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Sao chép kết quả đã publish từ stage 'build' vào image cuối cùng.
COPY --from=build /app/publish .

# Expose port 8080 và 8081 cho ứng dụng (HTTP/HTTPS theo mặc định của .NET 8)
EXPOSE 8080
EXPOSE 8081

# Entry point để chạy ứng dụng khi container khởi động.
ENTRYPOINT ["dotnet", "StudentActivities.dll"]
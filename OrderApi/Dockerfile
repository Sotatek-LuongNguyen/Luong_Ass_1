FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy toàn bộ source trước khi restore
COPY . .

WORKDIR "/src/OrderApi"

# Chạy restore trước
RUN dotnet restore "OrderApi.csproj"

# Build ứng dụng
RUN dotnet build "OrderApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Chạy ứng dụng
ENTRYPOINT ["dotnet", "OrderApi.dll"]

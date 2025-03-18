FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sao chép toàn bộ code trước để tránh lỗi file missing
COPY . .

WORKDIR "/src/PaymentApi"

# Restore package
RUN dotnet restore "PaymentApi.csproj"

# Build ứng dụng
RUN dotnet build "PaymentApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "PaymentApi.dll"]

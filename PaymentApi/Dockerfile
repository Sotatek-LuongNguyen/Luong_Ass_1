﻿

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PaymentApi.csproj", "./"]
RUN dotnet restore "./PaymentApi.csproj"
COPY . .


RUN dotnet build "PaymentApi.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "PaymentApi.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "PaymentApi.dll"]


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

COPY . .

RUN dotnet build "BankTransactionConciliationAPI/BankTransactionConciliationAPI.csproj" -c Release -o /app/build 
RUN dotnet publish "BankTransactionConciliationAPI/BankTransactionConciliationAPI.csproj" -c Release -o /app/publish

FROM base AS final
EXPOSE 5000
WORKDIR /app
COPY --from=build /app/publish /app

RUN ls /app

CMD ["dotnet", "/app/BankTransactionConciliationAPI.dll"]
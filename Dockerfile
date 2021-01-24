FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["EriRootSms.API.csproj", ""]
RUN dotnet restore "./EriRootSms.API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "EriRootSms.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "EriRootSms.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EriRootSms.API.dll"]
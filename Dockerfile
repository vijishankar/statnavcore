FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
 
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
RUN apt-get update && apt-get install -y curl
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get install -y nodejs
COPY . .
RUN dotnet build "StatNav.WebApplication\StatNav.WebApplication.csproj" -c Release -o /app/build
 
FROM build AS publish
RUN dotnet publish "StatNav.WebApplication\StatNav.WebApplication.csproj" -c Release -o /app/publish
 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StatNav.WebApplication.dll"]

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY *.sln .
# copy csproj and restore as distinct layers
COPY StatNav.WebApplication/StatNav.WebApplication.csproj ./StatNav.WebApplication/
RUN nuget restore

# copy everything else and build app
COPY StatNav.WebApplication/. ./StatNav.WebApplication/
WORKDIR /app/StatNav.WebApplication/
RUN msbuild /p:Configuration=Release -r:False


FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8 AS runtime
WORKDIR /inetpub/wwwroot

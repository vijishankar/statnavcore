FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build
WORKDIR /app

SHELL ["powershell"]
RUN iwr -useb get.scoop.sh | iex
RUN scoop install nodejs

COPY *.sln .
# copy csproj and restore as distinct layers
COPY StatNav.WebApplication/StatNav.WebApplication.csproj ./StatNav.WebApplication/
RUN nuget restore

# copy everything else and build app
COPY StatNav.WebApplication/. ./StatNav.WebApplication/
WORKDIR /app/StatNav.WebApplication/
RUN msbuild /p:Configuration=Release /p:SkipInvalidConfigurations=true


FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8 AS runtime
WORKDIR /inetpub/wwwroot

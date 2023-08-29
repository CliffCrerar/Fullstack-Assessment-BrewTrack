#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
RUN curl -sL https://deb.nodesource.com/setup_16.x | bash - && apt-get install -yq nodejs build-essential
WORKDIR /src
COPY ["BrewTrack.csproj", "."]
RUN dotnet restore "./BrewTrack.csproj"
COPY . .
RUN mv merged-appsettings.json appsettings.json
RUN dotnet build "BrewTrack.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BrewTrack.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BrewTrack.dll"]
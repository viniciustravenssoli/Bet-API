#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Bet.API/Bet.API.csproj", "Bet.API/"]
COPY ["Bet.Application/Bet.Application.csproj", "Bet.Application/"]
COPY ["Bet.Communication/Bet.Communication.csproj", "Bet.Communication/"]
COPY ["Bet.Domain/Bet.Domain.csproj", "Bet.Domain/"]
COPY ["Bet.Infra/Bet.Infra.csproj", "Bet.Infra/"]
RUN dotnet restore "./Bet.API/./Bet.API.csproj"
COPY . .
WORKDIR "/src/Bet.API"
RUN dotnet build "./Bet.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bet.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bet.API.dll"]
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BandHub.BandService/BandHub.BandService.csproj", "BandHub.BandService/"]
RUN dotnet restore "BandHub.BandService/BandHub.BandService.csproj"
COPY . .
WORKDIR "/src/BandHub.BandService"
RUN dotnet build "BandHub.BandService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BandHub.BandService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BandHub.BandService.dll"]

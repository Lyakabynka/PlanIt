FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PlanIt.ApiGateway.Ocelot/PlanIt.ApiGateway.Ocelot.csproj", "PlanIt.ApiGateway.Ocelot/"]
RUN dotnet restore "PlanIt.ApiGateway.Ocelot/PlanIt.ApiGateway.Ocelot.csproj"
COPY . .
WORKDIR "/src/PlanIt.ApiGateway.Ocelot"
RUN dotnet build "PlanIt.ApiGateway.Ocelot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlanIt.ApiGateway.Ocelot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlanIt.ApiGateway.Ocelot.dll"]

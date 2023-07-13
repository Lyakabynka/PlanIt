FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PlanIt.Plan.RestAPI/PlanIt.Plan.RestAPI.csproj", "PlanIt.Plan.RestAPI/"]
RUN dotnet restore "PlanIt.Plan.RestAPI/PlanIt.Plan.RestAPI.csproj"
COPY . .
WORKDIR "/src/PlanIt.Plan.RestAPI"
RUN dotnet build "PlanIt.Plan.RestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlanIt.Plan.RestAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlanIt.Plan.RestAPI.dll"]

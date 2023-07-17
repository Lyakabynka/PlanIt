FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PlanIt.Worker.RestAPI/PlanIt.Worker.RestAPI.csproj", "PlanIt.Worker.RestAPI/"]
RUN dotnet restore "PlanIt.Worker.RestAPI/PlanIt.Worker.RestAPI.csproj"
COPY . .
WORKDIR "/src/PlanIt.Worker.RestAPI"
RUN dotnet build "PlanIt.Worker.RestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlanIt.Worker.RestAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlanIt.Worker.RestAPI.dll"]

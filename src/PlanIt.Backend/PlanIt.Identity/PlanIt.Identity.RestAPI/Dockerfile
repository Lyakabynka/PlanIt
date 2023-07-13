FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PlanIt.Identity.RestAPI/PlanIt.Identity.RestAPI.csproj", "PlanIt.Identity.RestAPI/"]
RUN dotnet restore "PlanIt.Identity.RestAPI/PlanIt.Identity.RestAPI.csproj"
COPY . .
WORKDIR "/src/PlanIt.Identity.RestAPI"
RUN dotnet build "PlanIt.Identity.RestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlanIt.Identity.RestAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlanIt.Identity.RestAPI.dll"]

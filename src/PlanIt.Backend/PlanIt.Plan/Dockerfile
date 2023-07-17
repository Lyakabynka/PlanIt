FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

ARG POSTGRES_DB_USERNAME
ARG POSTGRES_DB_PASSWORD

ENV Database__User=$POSTGRES_DB_USERNAME
ENV Database__Password=$POSTGRES_DB_PASSWORD

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PlanIt.Plan.Domain/PlanIt.Plan.Domain.csproj", "PlanIt.Plan.Domain/PlanIt.Plan.Domain.csproj"]
COPY ["PlanIt.Plan.Application/PlanIt.Plan.Application.csproj", "PlanIt.Plan.Application/PlanIt.Plan.Application.csproj"]
COPY ["PlanIt.Plan.Persistence/PlanIt.Plan.Persistence.csproj", "PlanIt.Plan.Persistence/PlanIt.Plan.Persistence.csproj"]
COPY ["PlanIt.Plan.RestAPI/PlanIt.Plan.RestAPI.csproj", "PlanIt.Plan.RestAPI/PlanIt.Plan.RestAPI.csproj"]

COPY . .
WORKDIR "/src/PlanIt.Plan.RestAPI"
RUN dotnet build "PlanIt.Plan.RestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlanIt.Plan.RestAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlanIt.Plan.RestAPI.dll"]

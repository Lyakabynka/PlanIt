FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

ARG POSTGRES_DB_USERNAME
ARG POSTGRES_DB_PASSWORD

ENV Database__User=$POSTGRES_DB_USERNAME
ENV Database__Password=$POSTGRES_DB_PASSWORD

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PlanIt.Identity.Domain/PlanIt.Identity.Domain.csproj", "PlanIt.Identity.Domain/PlanIt.Identity.Domain.csproj"]
COPY ["PlanIt.Identity.Application/PlanIt.Identity.Application.csproj", "PlanIt.Identity.Application/PlanIt.Identity.Application.csproj"]
COPY ["PlanIt.Identity.Persistence/PlanIt.Identity.Persistence.csproj", "PlanIt.Identity.Persistence/PlanIt.Identity.Persistence.csproj"]
COPY ["PlanIt.Identity.RestAPI/PlanIt.Identity.RestAPI.csproj", "PlanIt.Identity.RestAPI/PlanIt.Identity.RestAPI.csproj"]

COPY . .
WORKDIR "/src/PlanIt.Identity.RestAPI"
RUN dotnet build "PlanIt.Identity.RestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlanIt.Identity.RestAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlanIt.Identity.RestAPI.dll"]
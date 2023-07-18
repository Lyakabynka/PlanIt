FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

ARG POSTGRES_DB_USERNAME
ARG POSTGRES_DB_PASSWORD

ENV Database__User=$POSTGRES_DB_USERNAME
ENV Database__Password=$POSTGRES_DB_PASSWORD

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PlanIt.Worker.Domain/PlanIt.Worker.Domain.csproj", "PlanIt.Worker.Domain/PlanIt.Worker.Domain.csproj"]
COPY ["PlanIt.Worker.Application/PlanIt.Worker.Application.csproj", "PlanIt.Worker.Application/PlanIt.Worker.Application.csproj"]
COPY ["PlanIt.Worker.Persistence/PlanIt.Worker.Persistence.csproj", "PlanIt.Worker.Persistence/PlanIt.Worker.Persistence.csproj"]
COPY ["PlanIt.Worker.RestAPI/PlanIt.Worker.RestAPI.csproj", "PlanIt.Worker.RestAPI/PlanIt.Worker.RestAPI.csproj"]

COPY . .
WORKDIR "/src/PlanIt.Worker.RestAPI"
RUN dotnet build "PlanIt.Worker.RestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlanIt.Worker.RestAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlanIt.Worker.RestAPI.dll"]

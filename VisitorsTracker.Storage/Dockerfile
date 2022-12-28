FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY ["VisitorsTracker.Storage/VisitorsTracker.Storage.csproj", "VisitorsTracker.Storage/"]
COPY ["VisitorsTracker.Common/VisitorsTracker.Common.csproj", "VisitorsTracker.Common/"]

RUN dotnet restore "VisitorsTracker.Storage/VisitorsTracker.Storage.csproj"

COPY . .
WORKDIR "/app/VisitorsTracker.Storage"
RUN dotnet publish "VisitorsTracker.Storage.csproj" -c Release -o /publish --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /publish ./
ENTRYPOINT ["dotnet", "Storage.dll"]
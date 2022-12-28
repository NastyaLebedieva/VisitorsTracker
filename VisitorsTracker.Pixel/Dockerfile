FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY ["VisitorsTracker.Pixel/VisitorsTracker.Pixel.csproj", "VisitorsTracker.Pixel/"]
COPY ["VisitorsTracker.Common/VisitorsTracker.Common.csproj", "VisitorsTracker.Common/"]

RUN dotnet restore "VisitorsTracker.Pixel/VisitorsTracker.Pixel.csproj"

COPY . .
WORKDIR "/app/VisitorsTracker.Pixel"
RUN dotnet publish "VisitorsTracker.Pixel.csproj" -c Release -o /publish --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /publish ./
ENTRYPOINT ["dotnet", "Pixel.dll"]
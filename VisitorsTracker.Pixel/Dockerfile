FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Pixel/Pixel.csproj", "Pixel/"]
RUN dotnet restore "Pixel/Pixel.csproj"
COPY . .
WORKDIR "/src/Pixel"
RUN dotnet build "Pixel.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pixel.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pixel.dll"]

#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS build
#WORKDIR /app
#COPY . .
#RUN dotnet restore "Pixel/Pixel.csproj"
#RUN dotnet publish "Pixel.csproj" -c Release -o out
#
#FROM mcr.microsoft.com/dotnet/aspnet:6.0
#WORKDIR /app
#COPY --from=build /app/out .
#ENTRYPOINT ["dotnet", "Pixel.dll"]
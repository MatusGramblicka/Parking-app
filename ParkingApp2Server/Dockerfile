FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build

WORKDIR /app

COPY ./ ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim

WORKDIR /app

COPY --from=build /app/out .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet ParkingApp2Server.dll
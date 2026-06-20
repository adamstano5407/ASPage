FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY APIKros/APIKros.csproj APIKros/
RUN dotnet restore APIKros/APIKros.csproj

COPY . .
WORKDIR /src/APIKros
RUN dotnet publish APIKros.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "APIKros.dll"]
FROM mcr.microsoft.com/dotnet/sdk:10.0

RUN apt-get update \
    && apt-get install -y gosu \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src

RUN dotnet tool install --tool-path /usr/local/bin dotnet-ef
RUN dotnet tool install --tool-path /usr/local/bin TeaPie.Tool

COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

EXPOSE 8080
ENTRYPOINT ["/entrypoint.sh"]


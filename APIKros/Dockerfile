FROM mcr.microsoft.com/dotnet/sdk:10.0

RUN apt-get update \
    && apt-get install -y gosu \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src/APIKros
RUN dotnet tool install --global dotnet-ef
RUN dotnet tool install --global TeaPie.Tool

ENV PATH="${PATH}:/root/.dotnet/tools"

COPY APIKros/entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

EXPOSE 8080

ENTRYPOINT ["/entrypoint.sh"]
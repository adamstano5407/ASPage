#!/bin/bash
set -e

echo "Fixing permissions..."

mkdir -p \
    /src/bin \
    /src/obj \
    /src/Tests \
    /src/Migrations \
    /home/ubuntu/.nuget/packages \
    /home/ubuntu/.nuget/NuGet \
    /home/ubuntu/.dotnet/tools

chown -R 1000:1000 \
    /src/bin \
    /src/obj \
    /src/Tests \
    /src/Migrations \
    /home/ubuntu/.nuget \
    /home/ubuntu/.dotnet || true

echo "Starting app as user 1000:1000"

exec gosu 1000:1000 env HOME=/home/ubuntu \
    PATH="/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin:/home/ubuntu/.dotnet/tools" \
    bash -lc '
dotnet restore
dotnet watch run --urls http://0.0.0.0:8080
'
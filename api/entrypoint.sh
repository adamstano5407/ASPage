#!/bin/bash
set -e

USER_ID=${LOCAL_UID:-1000}
GROUP_ID=${LOCAL_GID:-1000}

echo "Fixing permissions..."

mkdir -p \
    /src/bin \
    /src/obj \
    /src/Tests \
    /src/Migrations \
    /home/ubuntu/.nuget/packages \
    /home/ubuntu/.nuget/NuGet \
    /home/ubuntu/.dotnet/tools

chown -R ${USER_ID}:${GROUP_ID} \
    /src/bin \
    /src/obj \
    /src/Tests \
    /src/Migrations \
    /home/ubuntu/.nuget \
    /home/ubuntu/.dotnet || true

echo "Starting app as user ${USER_ID}:${GROUP_ID}"

exec gosu ${USER_ID}:${GROUP_ID} env HOME=/home/ubuntu \
    PATH="/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin:/home/ubuntu/.dotnet/tools" \
    bash -lc '
dotnet restore
dotnet watch run --urls http://0.0.0.0:8080
'
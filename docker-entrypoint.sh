#!/bin/sh

set -e

groupadd --gid ${NETCORE_USER_UID} netcore && useradd --uid ${NETCORE_USER_UID} -g netcore netcore

chmod -R u=rwX,g=rX,o= /app/
chown root:netcore /app/
chown -RL root:netcore /app/bin/

sudo -E -u netcore dotnet itec-mobile-api-final.dll
exec sudo -E -u netcore dotnet itec-mobile-api-final.dll
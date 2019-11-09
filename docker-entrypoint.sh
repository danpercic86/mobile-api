#!/bin/sh

set -e

groupadd --gid ${NETCORE_USER_UID} netcore && useradd --uid ${NETCORE_USER_UID} -g netcore netcore


chmod -R u=rwX,g=rX,o= /app/
chown root:netcore /app/
chown -RL root:netcore /app/bin/


mkdir -p ${CONTENT_DIRECTORY} ${LOGS_DIRECTORY} ${TEMPORARY_VIEWS_PATH}
chmod -R u=rwX,g=rX,o= ${CONTENT_DIRECTORY} ${LOGS_DIRECTORY} ${TEMPORARY_VIEWS_PATH}
chown -RL netcore:netcore ${CONTENT_DIRECTORY} ${LOGS_DIRECTORY} ${TEMPORARY_VIEWS_PATH}

sudo -E -u netcore dotnet itec-mobile-api-final.dll --migrate true --seed true
exec sudo -E -u netcore dotnet itec-mobile-api-final.dll



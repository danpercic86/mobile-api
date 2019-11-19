FROM microsoft/dotnet:2.2-sdk as build-env

WORKDIR /app/src/

COPY itec-mobile-api-final.csproj itec-mobile-api-final.sln ./
RUN dotnet restore

COPY . .
RUN dotnet publish --output "/app/bin" --configuration release
RUN dotnet ef database update

FROM microsoft/dotnet:2.2-aspnetcore-runtime as runtime-env
RUN apt-get update && apt-get install -y \
    sudo \
    net-tools \
 && rm -rf /var/lib/apt/lists/*

WORKDIR /app/bin
EXPOSE 5030
ENV NETCORE_USER_UID 69

COPY docker-entrypoint.sh /usr/bin/docker-entrypoint.sh
RUN chmod +x /usr/bin/docker-entrypoint.sh
CMD ["docker-entrypoint.sh"]

COPY --from=build-env /app/bin /app/bin

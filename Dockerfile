FROM microsoft/dotnet:2.1.301-sdk-alpine3.7 AS builder
ENV IS_DOCKER_ENV=true
WORKDIR /source
COPY . .
RUN apk --no-cache add bash
RUN dotnet restore
RUN dotnet publish --output /app/ --configuration Release

FROM microsoft/dotnet:2.1.1-aspnetcore-runtime-alpine3.7
WORKDIR /app
COPY --from=builder /app .
CMD ["dotnet", "Consensus.dll"]

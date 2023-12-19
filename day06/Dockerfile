FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine as builder
WORKDIR /aoc
RUN apk add clang build-base zlib-dev
COPY ./Day06.csproj .
RUN dotnet restore -r linux-musl-x64
COPY ./input.txt .
COPY ./Program.cs .
RUN dotnet publish --no-restore -c Release -r linux-musl-x64 -o .

FROM alpine as publish
WORKDIR /
RUN apk add libstdc++
COPY --from=builder /aoc/Day06 .
ENTRYPOINT /Day06

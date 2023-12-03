FROM alpine as build-env
RUN apk add --no-cache build-base
WORKDIR /app
COPY ./day03.c ./
RUN gcc day03.c -o day03

FROM alpine
WORKDIR /app
COPY ./input.txt ./
COPY --from=build-env /app/day03 ./
CMD ["./day03"]

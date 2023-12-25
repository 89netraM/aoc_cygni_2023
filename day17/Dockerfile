FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /aoc
RUN git clone https://github.com/89netraM/Advent-of-Code.git .
WORKDIR /aoc/2023
RUN dotnet build -c Release
RUN ln /bin/true /bin/xsel
ENTRYPOINT "/bin/pwsh" "-c" "dotnet run --no-build -c Release -- 17 \$(if (\$Env:part -eq 'part1') {'1'} else {'2'})"

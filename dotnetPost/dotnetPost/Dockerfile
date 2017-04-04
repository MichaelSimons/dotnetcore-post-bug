FROM microsoft/dotnet

WORKDIR app
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet build
RUN dotnet publish -o out

ENTRYPOINT dotnet run out/dotnetPost


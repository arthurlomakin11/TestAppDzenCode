FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN apt-get update && apt-get install -y apt-utils libgdiplus libc6-dev

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# Install NodeJs
RUN apt-get update && \
apt-get install -y wget && \
apt-get install -y gnupg2 && \
wget -qO- https://deb.nodesource.com/setup_16.x | bash - && \
apt-get install -y nodejs
# End Install
WORKDIR /src
COPY ["TestAppDzenCode.csproj", "./"]
RUN dotnet restore "TestAppDzenCode.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "TestAppDzenCode.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestAppDzenCode.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestAppDzenCode.dll"]

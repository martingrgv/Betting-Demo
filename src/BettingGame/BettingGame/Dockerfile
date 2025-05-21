FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BettingGame/BettingGame.UI/BettingGame.UI.csproj", "BettingGame/BettingGame.UI/"]
COPY ["BettingGame/BettingGame.Domain/BettingGame.Domain.csproj", "BettingGame/BettingGame.Domain/"]
COPY ["BettingGame/BettingGame.Shared/BettingGame.Shared.csproj", "BettingGame/BettingGame.Shared/"]
COPY ["BettingGame/BettingGame.Infrastructure/BettingGame.Infrastructure.csproj", "BettingGame/BettingGame.Infrastructure/"]
COPY ["BettingGame/BettingGame.Application/BettingGame.Application.csproj", "BettingGame/BettingGame.Application/"]
RUN dotnet restore "BettingGame/BettingGame.UI/BettingGame.UI.csproj"
COPY . .
WORKDIR "/src/BettingGame/BettingGame.UI"
RUN dotnet build "BettingGame.UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BettingGame.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BettingGame.UI.dll"]

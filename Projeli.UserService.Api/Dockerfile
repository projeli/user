FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["user/Projeli.UserService.Api/Projeli.UserService.Api.csproj", "user/Projeli.UserService.Api/"]
COPY ["shared/Projeli.Shared.Domain/Projeli.Shared.Domain.csproj", "shared/Projeli.Shared.Domain/"]
COPY ["shared/Projeli.Shared.Application/Projeli.Shared.Application.csproj", "shared/Projeli.Shared.Application/"]
COPY ["shared/Projeli.Shared.Infrastructure/Projeli.Shared.Infrastructure.csproj", "shared/Projeli.Shared.Infrastructure/"]
RUN dotnet restore "user/Projeli.UserService.Api/Projeli.UserService.Api.csproj"
COPY ["user", "user"]
COPY ["shared", "shared"]
WORKDIR "/src/user/Projeli.UserService.Api"
RUN dotnet build "Projeli.UserService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Projeli.UserService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Projeli.UserService.Api.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY src/ ./

RUN dotnet restore "SimpleBlogApi/SimpleBlogApi.csproj"
RUN dotnet build "SimpleBlogApi/SimpleBlogApi.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "SimpleBlogApi/SimpleBlogApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SimpleBlogApi/SimpleBlogApi.dll"]
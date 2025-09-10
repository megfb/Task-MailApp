FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7135
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["backend/TextboxMailApp/TextboxMailApp.Api/TextboxMailApp.Api.csproj", "TextboxMailApp.Api/"]
COPY ["backend/TextboxMailApp/TextboxMailApp.Persistence/TextboxMailApp.Persistence.csproj", "TextboxMailApp.Persistence/"]
COPY ["backend/TextboxMailApp/TextboxMailApp.Application/TextboxMailApp.Application.csproj", "TextboxMailApp.Application/"]
COPY ["backend/TextboxMailApp/TextboxMailApp.Domain/TextboxMailApp.Domain.csproj", "TextboxMailApp.Domain/"]
RUN dotnet restore "./TextboxMailApp.Api/TextboxMailApp.Api.csproj" --source https://api.nuget.org/v3/index.json
COPY ["backend/TextboxMailApp/TextboxMailApp.Api/", "TextboxMailApp.Api/"]
COPY ["backend/TextboxMailApp/TextboxMailApp.Persistence/", "TextboxMailApp.Persistence/"]
COPY ["backend/TextboxMailApp/TextboxMailApp.Application/", "TextboxMailApp.Application/"]
COPY ["backend/TextboxMailApp/TextboxMailApp.Domain/", "TextboxMailApp.Domain/"]
# Kopyalanan dosyaları kontrol et
RUN ls -R /src
WORKDIR "/src/TextboxMailApp.Api"
RUN dotnet publish "./TextboxMailApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
# Publish dizinini kontrol et
RUN ls -R /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TextboxMailApp.Api.dll"]
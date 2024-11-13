# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80  # Port for production use

# Optionally set ASPNETCORE_ENVIRONMENT to Production
# This ensures the environment is set to 'Production' when the container runs
ENV ASPNETCORE_ENVIRONMENT=Production

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RecipeFinder WebApp/RecipeFinder WebApp.csproj", "RecipeFinder WebApp/"]
COPY ["Recipe Finder/Recipe Finder.csproj", "Recipe Finder/"]
RUN dotnet restore "./RecipeFinder WebApp/RecipeFinder WebApp.csproj"
COPY . . 
WORKDIR "/src/RecipeFinder WebApp"
RUN dotnet build "./RecipeFinder WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RecipeFinder WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecipeFinder_WebApp.dll"]

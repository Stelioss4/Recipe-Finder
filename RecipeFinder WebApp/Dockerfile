# Base Image for ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image using .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RecipeFinder WebApp/RecipeFinder WebApp.csproj", "RecipeFinder WebApp/"]
COPY ["Recipe Finder/Recipe Finder.csproj", "Recipe Finder/"]
RUN dotnet restore "RecipeFinder WebApp/RecipeFinder WebApp.csproj"
COPY . .
WORKDIR "/src/RecipeFinder WebApp"
RUN dotnet build "RecipeFinder WebApp.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "RecipeFinder WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image to run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecipeFinder WebApp.dll"]

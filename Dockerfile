# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy entire source
COPY . .

# Restore and publish
# AFTER (recommended)
RUN dotnet restore "./ChatService.sln" && \
    dotnet publish "./src/ChatService/ChatService.csproj" -c Release -o /app/publish

# RUN dotnet restore "./ChatService.sln"
# RUN dotnet publish "./src/ChatService/ChatService.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ChatService.dll"]

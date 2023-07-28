FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5139

ENV ASPNETCORE_URLS=http://+:5139

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Grupo2_Identity_Server/Grupo2_Identity_Server.csproj", "Grupo2_Identity_Server/"]
RUN dotnet restore "Grupo2_Identity_Server/Grupo2_Identity_Server.csproj"
COPY . .
WORKDIR "/src/Grupo2_Identity_Server"
RUN dotnet build "Grupo2_Identity_Server.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Grupo2_Identity_Server.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Grupo2_Identity_Server.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["upload_backend/upload_backend.csproj", "upload_backend/"]
RUN dotnet restore "upload_backend/upload_backend.csproj"
COPY . .
WORKDIR "/src/upload_backend"
RUN dotnet build "upload_backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "upload_backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "upload_backend.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TbcTaskApi/TbcTaskApi.csproj", "TbcTaskApi/"]
RUN dotnet restore "TbcTaskApi/TbcTaskApi.csproj"
COPY . .
WORKDIR "/src/TbcTaskApi"
RUN dotnet build "TbcTaskApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TbcTaskApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TbcTaskApi.dll"]

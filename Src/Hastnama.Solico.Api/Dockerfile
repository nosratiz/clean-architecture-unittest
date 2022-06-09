FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Src/Hastnama.Solico.Api/Hastnama.Solico.Api.csproj", "Src/Hastnama.Solico.Api/"]
COPY ["Src/Hastnama.Solico.Persistence/Hastnama.Solico.Persistence.csproj", "Src/Hastnama.Solico.Persistence/"]
COPY ["Src/Hastnama.Solico.Application/Hastnama.Solico.Application.csproj", "Src/Hastnama.Solico.Application/"]
COPY ["Src/Hastnama.Solico.Common/Hastnama.Solico.Common.csproj", "Src/Hastnama.Solico.Common/"]
COPY ["Src/Hastnama.Solico.Domain/Hastnama.Solico.Domain.csproj", "Src/Hastnama.Solico.Domain/"]
RUN dotnet restore "Src/Hastnama.Solico.Api/Hastnama.Solico.Api.csproj"
COPY . .
WORKDIR "/src/Src/Hastnama.Solico.Api"
RUN dotnet build "Hastnama.Solico.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hastnama.Solico.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hastnama.Solico.Api.dll"]

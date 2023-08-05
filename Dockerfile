FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["src/MockAuth0.Api/MockAuth0.Api.csproj", "MockAuth0.Api/"]
COPY ["src/MockAuth0.Api/Forms/", "MockAuth0.Api/forms"]


RUN dotnet restore "MockAuth0.Api/MockAuth0.Api.csproj"
COPY src/. .
WORKDIR "/src/MockAuth0.Api"
RUN dotnet build "MockAuth0.Api.csproj" -c Release -o /app/build

FROM build AS publish 
RUN dotnet publish "MockAuth0.Api.csproj" -c Release -o /app/publish

FROM base AS final
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS=https://+:5000
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/configs/cert.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=changeit

RUN apk add --no-cache icu-libs

EXPOSE 5000
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/configs

CMD ["dotnet", "MockAuth0.Api.dll"]

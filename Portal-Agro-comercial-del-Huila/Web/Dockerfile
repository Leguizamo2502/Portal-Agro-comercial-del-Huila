# Consulte https://aka.ms/customizecontainer para aprender a personalizar su contenedor de depuración y cómo Visual Studio usa este Dockerfile para compilar sus imágenes para una depuración más rápida.

# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
#EXPOSE 8081


# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Portal-Agro-comercial-del-Huila/Web/Web.csproj Web/
COPY Portal-Agro-comercial-del-Huila/Business/Business.csproj Business/
COPY Portal-Agro-comercial-del-Huila/Custom/Custom.csproj Custom/
COPY Portal-Agro-comercial-del-Huila/Data/Data.csproj Data/
COPY Portal-Agro-comercial-del-Huila/Entity/Entity.csproj Entity/
COPY Portal-Agro-comercial-del-Huila/Utilities/Utilities.csproj Utilities/
#COPY Portal-Agro-comercial-del-Huila/Portal-Agro.sln .
COPY . .


RUN dotnet restore "./Web/Web.csproj"

# Copiar resto del código
COPY Portal-Agro-comercial-del-Huila/ .


WORKDIR "/src/Web"
RUN dotnet build "./Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase se usa para publicar el proyecto de servicio que se copiará en la fase final.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]
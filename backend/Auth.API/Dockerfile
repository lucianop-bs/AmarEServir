
# Estágio base: Configuração do ambiente de runtime
# Usa a imagem Alpine do ASP.NET Core 9.0 para reduzir o tamanho da imagem
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
# Expõe a porta 8080 para acesso HTTP
EXPOSE 8080

# Instala bibliotecas ICU necessárias para internacionalização e localização
RUN apk add --no-cache icu-libs
# Define que o .NET deve usar as configurações de globalização do sistema operacional
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
# Executa o container com o usuário não-root padrão para maior segurança
USER $APP_UID

# Estágio de build: Compilação da aplicação
# Usa a imagem Alpine do SDK .NET 9.0 com ferramentas de compilação
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src/Auth.API
# Copia os arquivos de projeto para o container
COPY ["Auth.API/Auth.API.csproj", "Auth.API/"]
COPY ["Core/AmarEServir.Core.csproj", "Core/"]
# Restaura as dependências do projeto
RUN dotnet restore "Auth.API/Auth.API.csproj"

# Copia todo o código fonte para o container
COPY . .
# Compila a aplicação em modo Release
RUN dotnet build "Auth.API/Auth.API.csproj" -c Release -o ../app/build

# Estágio de publicação: Preparação dos artefatos finais
FROM build AS publish
# Publica a aplicação otimizada para produção
# UseAppHost=false evita a criação de um executável nativo
RUN dotnet publish "Auth.API/Auth.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio final: Imagem de produção
# Usa o estágio base (menor e mais seguro) como base final
FROM base AS final
WORKDIR /app
# Copia os artefatos publicados do estágio anterior
COPY --from=publish /app/publish .
# Define o ponto de entrada da aplicação
ENTRYPOINT ["dotnet", "Auth.API.dll"]
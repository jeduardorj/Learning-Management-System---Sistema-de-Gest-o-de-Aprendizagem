# ==============================================
# ESTÁGIO 1: BUILD
# Usa o SDK completo para restaurar e compilar
# ==============================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto primeiro (melhor uso do cache do Docker)
# Se só o código mudar mas não os .csproj, o restore é cacheado
COPY ["src/LMS.API/LMS.API.csproj", "src/LMS.API/"]
COPY ["src/LMS.Application/LMS.Application.csproj", "src/LMS.Application/"]
COPY ["src/LMS.Domain/LMS.Domain.csproj", "src/LMS.Domain/"]
COPY ["src/LMS.Infrastructure/LMS.Infrastructure.csproj", "src/LMS.Infrastructure/"]

# Restaura as dependências NuGet
RUN dotnet restore "src/LMS.API/LMS.API.csproj"

# Copia o restante do código fonte
COPY . .

# Publica o projeto em modo Release
RUN dotnet publish "src/LMS.API/LMS.API.csproj" -c Release -o /app/publish --no-restore

# ==============================================
# ESTÁGIO 2: RUNTIME
# Usa só o runtime — imagem menor e mais segura
# ==============================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Cria pasta de logs
RUN mkdir -p logs

# Copia apenas o resultado do publish do estágio anterior
COPY --from=build /app/publish .

# Expõe a porta da API
EXPOSE 8080

# Variável de ambiente para o ASP.NET usar a porta 8080
ENV ASPNETCORE_URLS=http://+:8080

# Ponto de entrada da aplicação
ENTRYPOINT ["dotnet", "LMS.API.dll"]

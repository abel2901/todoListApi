# Usar a imagem base do .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Setar o diretório de trabalho no container
WORKDIR /app

# Copiar o arquivo de projeto e restaurar as dependências
COPY *.csproj .
RUN dotnet restore

# Copiar todo o código e construir a aplicação
COPY . .
RUN dotnet publish -c Release -o out

# Gerar a imagem final para a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
COPY --from=build /app/out . 

# Expor a porta em que a API vai rodar
EXPOSE 80 443

# Comando para rodar a aplicação
ENTRYPOINT ["dotnet", "TodoList.dll"]

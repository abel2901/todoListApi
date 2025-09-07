# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copiar csproj e restaurar dependências
COPY *.csproj ./
RUN dotnet restore

# Copiar todo o código e compilar
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa final (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Definir variáveis de ambiente
ENV ASPNETCORE_ENVIRONMENT=Production
# Railway usa a variável PORT para expor o serviço
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_USE_POLLING_FILE_WATCHER=true

# Conexão com o banco (precisa configurar no painel do Railway)
# Exemplo: se for PostgreSQL, configure DATABASE_URL lá e use no Program.cs
# ENV ConnectionStrings__TarefaConnection=${DATABASE_URL}

# Copiar saída do build
COPY --from=build /app/out .

# Expõe a porta dinâmica que o Railway define
EXPOSE 80

# EntryPoint
ENTRYPOINT ["dotnet", "TodoList.dll"]

# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copia tudo da pasta 'backend' para dentro do container
COPY . ./

# Restaura e publica sem precisar nomear o arquivo .csproj (ele pega o que estiver na pasta)
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Estágio 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia os arquivos compilados do estágio de build
COPY --from=build /app/out .

# Configuração de porta para o Render
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# ATENÇÃO: O nome da DLL deve ser idêntico ao do arquivo .csproj
# Se o arquivo é chatIa.csproj, a saída será chatIa.dll
ENTRYPOINT ["dotnet", "chatIa.dll"]
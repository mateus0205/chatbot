# 1. Estágio de Build (SDK 8.0)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia tudo da pasta 'backend' para o container
COPY . ./

# Restaura as dependências (ele vai encontrar o 'chat.csproj' automaticamente)
RUN dotnet restore

# Publica o projeto. O output será 'chat.dll'
RUN dotnet publish -c Release -o out /p:UseAppHost=false

# 2. Estágio de Runtime (ASP.NET 8.0)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia apenas os arquivos compilados do estágio anterior
COPY --from=build /app/out .

# Configura a porta para o Render
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# O comando para rodar a aplicação. 
# Como o projeto é 'chat.csproj', a DLL gerada é 'chat.dll'
ENTRYPOINT ["dotnet", "chat.dll"]
# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia tudo e publica
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out /p:UseAppHost=false

# Estágio de Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expõe a porta para o Render
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# O SEGREDO: Usar um script para rodar a DLL correta automaticamente
ENTRYPOINT ["sh", "-c", "dotnet $(ls *.dll | head -n 1)"]
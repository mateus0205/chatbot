# 1. Estágio de Build (Compilação)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o ficheiro de projeto e restaura as dependências
COPY ["chatIa.csproj", "./"]
RUN dotnet restore "chatIa.csproj"

# Copia todo o resto do código e compila
COPY . .
RUN dotnet publish "chatIa.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Estágio Final (Runtime - Execução)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Define a porta padrão do ASP.NET Core para o Render/Railway
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "chatIa.dll"]
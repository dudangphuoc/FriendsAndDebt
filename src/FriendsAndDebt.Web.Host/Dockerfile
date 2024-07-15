FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["src/FriendsAndDebt.Web.Host/FriendsAndDebt.Web.Host.csproj", "src/FriendsAndDebt.Web.Host/"]
COPY ["src/FriendsAndDebt.Web.Core/FriendsAndDebt.Web.Core.csproj", "src/FriendsAndDebt.Web.Core/"]
COPY ["src/FriendsAndDebt.Application/FriendsAndDebt.Application.csproj", "src/FriendsAndDebt.Application/"]
COPY ["src/FriendsAndDebt.Core/FriendsAndDebt.Core.csproj", "src/FriendsAndDebt.Core/"]
COPY ["src/FriendsAndDebt.EntityFrameworkCore/FriendsAndDebt.EntityFrameworkCore.csproj", "src/FriendsAndDebt.EntityFrameworkCore/"]
WORKDIR "/src/src/FriendsAndDebt.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/FriendsAndDebt.Web.Host", "src/FriendsAndDebt.Web.Host"]
COPY ["src/FriendsAndDebt.Web.Core", "src/FriendsAndDebt.Web.Core"]
COPY ["src/FriendsAndDebt.Application", "src/FriendsAndDebt.Application"]
COPY ["src/FriendsAndDebt.Core", "src/FriendsAndDebt.Core"]
COPY ["src/FriendsAndDebt.EntityFrameworkCore", "src/FriendsAndDebt.EntityFrameworkCore"]
WORKDIR "/src/src/FriendsAndDebt.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "FriendsAndDebt.Web.Host.dll"]

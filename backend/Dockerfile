FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source
COPY ["VKVideoReviews.sln", "."]
COPY ["src/VKVideoReviews.WebApi/VKVideoReviews.WebApi.csproj", "src/VKVideoReviews.WebApi/"]
COPY ["src/VKVideoReviews.BL/VKVideoReviews.BL.csproj", "src/VKVideoReviews.BL/"]
COPY ["src/VKVideoReviews.DA/VKVideoReviews.DA.csproj", "src/VKVideoReviews.DA/"]

RUN dotnet restore "VKVideoReviews.sln"

COPY . .

WORKDIR "/source/src/VKVideoReviews.WebApi"
RUN dotnet publish "VKVideoReviews.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "VKVideoReviews.WebApi.dll"]


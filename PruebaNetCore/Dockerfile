FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR webapp

EXPOSE 80
EXPOSE 7082

#COPY PROJECT FILES
COPY ./*.csproj ./
RUN dotnet restore

#COPY EVERYTHING ELSE
COPY . .
RUN dotnet publish -c Release -o out

#BUild image
FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /webapp
COPY --from=build /webapp/out .
ENTRYPOINT ["dotnet", "PruebaNetCore.dll"]
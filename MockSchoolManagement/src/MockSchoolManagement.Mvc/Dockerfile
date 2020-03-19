#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src
COPY ["src/MockSchoolManagement.Mvc/MockSchoolManagement.Mvc.csproj", "src/MockSchoolManagement.Mvc/"]
COPY ["src/MockSchoolManagement.Application/MockSchoolManagement.Application.csproj", "src/MockSchoolManagement.Application/"]
COPY ["src/MockSchoolManagement.EntityFrameworkCore/MockSchoolManagement.EntityFrameworkCore.csproj", "src/MockSchoolManagement.EntityFrameworkCore/"]
COPY ["src/MockSchoolManagement.Core/MockSchoolManagement.Core.csproj", "src/MockSchoolManagement.Core/"]
RUN dotnet restore "src/MockSchoolManagement.Mvc/MockSchoolManagement.Mvc.csproj"
COPY . .
WORKDIR "/src/src/MockSchoolManagement.Mvc"
RUN dotnet build "MockSchoolManagement.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MockSchoolManagement.Mvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MockSchoolManagement.Mvc.dll"]
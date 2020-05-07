# 设置运行时
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base

COPY . .

## 设置为APP为项目路径
WORKDIR /app
## 开放80端口
EXPOSE 80

ENTRYPOINT ["dotnet", "MockSchoolManagement.Mvc.dll"]

 
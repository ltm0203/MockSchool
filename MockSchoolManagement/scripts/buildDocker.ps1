# 以上脚本均需要在powershell中使用

$buildFolder = (Get-Item -Path "./" -Verbose).FullName ## 当前文件所在目录
$slnFolder = Join-Path $buildFolder "../"   ## 当前解决方案所在文件夹
$outputFolder = Join-Path $buildFolder "outputs" ## 当前文件夹下的outputs文件夹
$webMvcFolder = Join-Path $slnFolder "src/MockSchoolManagement.Mvc"  ## webMVC项目所在文件夹路径
 

Write-Host $buildFolder
Write-Host $slnFolder
Write-Host $outputFolder
Write-Host $webMvcFolder


## 删除outpus文件夹 ######################################################################

Remove-Item $outputFolder -Force -Recurse -ErrorAction Ignore

## 删除后再创建一个新的空文件夹
New-Item -Path $outputFolder -ItemType Directory


# 设置路径到解决方案文件夹  ###################################################
Set-Location $webMvcFolder

# 还原项目依赖的包###################################################
dotnet restore 


## 发布WebMVC的项目，发布的路径为当前脚本夹下的outputs/MVC中 ###################################################

Set-Location $webMvcFolder
dotnet publish --output (Join-Path $outputFolder "Mvc") --configuration Release   --no-restore

## 创建Docker镜像 #######################################################

# Mvc
Set-Location (Join-Path $outputFolder "Mvc")

docker rmi ltm/mockschool -f
docker build -t ltm/mockschool .


# dotnet publish  --configuration Release --output dist
#docker build . -t ltm/exampleapp -f Dockerfile

# cd C:\Code\github\MockSchool\MockSchoolManagement\scripts

# C:\Code\github\MockSchool\MockSchoolManagement\scripts\outputs\Mvc\
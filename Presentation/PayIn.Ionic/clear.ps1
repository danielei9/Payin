##################################
#          Clear Ionic           #
##################################

#Rutas
$projectDir = "C:\Repos\PayIn\Presentation\PayIn.Ionic"
$platformsDir = "\platforms"
$pluginsDir= "\plugins"
$resourceAndroidDir = "resources\android"
$resourceIosDir = "resources\ios"

# Array
$resourcesArray = @(
($platformsDir  = "\platforms"),
($pluginsDir = "\plugins"),
($resourceAndroidDir = "\resources\android"),
($resourceIosDir = "\resources\ios"))

# Navegación a ruta del proyecto
Set-Location -Path $projectDir

# Comandos ionic
ionic platforms rm android
Write-Host "Ionic Android platform removed"

ionic platforms rm ios  
Write-Host "Ionic IOS platform removed"

# Elimincación de directorios
foreach ($resource in $resourcesArray)
{
    $path = $projectDir + $resource
    $resourceExists = Get-Item -Path $path -erroraction 'silentlycontinue'
    if ($resourceExists)
    {
           Remove-Item -Path $path -Force -Recurse
           Write-Host "Resource " $resource " deleted" -ForegroundColor Green
    }
    else
    {
        Write-Host "Resource " $resource " don't exists" -ForegroundColor Yellow
    }
}

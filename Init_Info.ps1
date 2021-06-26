# --------- Script para la preparación del entorno Azure ---------- #
# Builder: Israel Pérez - israel.perez@pay-in.es 
# Date: 8/3/2016

# **********************************
# 1 - Preparar el entorno PowerShell
# ********************************** 

# Install the Azure Resource Manager modules from the PowerShell Gallery
Install-Module AzureRM
Install-AzureRM

# Install the Azure Service Management module from the PowerShell Gallery
Install-Module Azure

# Import AzureRM modules for the given version manifest in the AzureRM module
# Import-AzureRM

# Import Azure Service Management module
# Import-Module Azure

# Listado de módulos - Verificar si están los módulos requeridos
Get-Module –ListAvailable 


# ****************************************************************************************
# 2 - Módulo Azure Active Directory - instalar los siguientes ficheros en orden:
#     Link -> https://technet.microsoft.com/en-us/library/jj151815.aspx#bkmk_installmodule
# ****************************************************************************************

# msoidcli_64.msi
# esAADModule.msi


# ***********************************************************************
# 3 - Modificación de las políticas de ejecucion de scripts en PowerShell
# ***********************************************************************

# Para poder ejecutar scripts en powershell es necesario modificar la variable ExecutionPolicy
# Verificar la politia de ejecución de powershell -> Get-ExecutionPolicy
# Cambiar la política -> Set-ExecutionPolicy Unrestricted
# Nota: Por seguridad volver a asignar la polítia a RemoteSigned 
# (Solo se ejecutarán los scripts creados en la propia máquina)


# ******************************************************************************************
# 4 - Comando para instalar la librería de autenticación AD en Package Manager Console de VS
# ******************************************************************************************

# Install-Package Microsoft.IdentityModel.Clients.ActiveDirectory -Version 2.23.302261847


# *******************************************************************************
# 5 - Comando para instalar la librería KeyVault en Package Manager Console de VS
# *******************************************************************************

# Install-Package Microsoft.Azure.KeyVault

# *******************************************************************************
# 6 - Rquisitos previos 
# *******************************************************************************

# Si es la primera vez que se ejecutaan los scripts ha de estar montada una estructura Azure Active Directory para almacenar 
# al nuevo usuario aplicación y obtener el ObjectId del Administrador para asignarle los permisos correspondientes


# *************************************
# 7 - Orden de ejecución de los scripts
# *************************************

# GetAppConfigSettings.ps1
# GetServiceCOnfigSettings.ps1
# SecretCreationScript.ps1
# Nota: Es necesario revisar los scripts para completar los valores de las variables que sean neccesarios

# ************************************************************************************************************************************
# OPCIONAL - Asignación de permisos de lectura a un usuario en el grupo de recursos - necesario para permitir el acceso a 3os usuarios
# ************************************************************************************************************************************

# New-AzureRmRoleAssignment -Mail keyvaultuser@domain.onmicrosoft.com -RoleDefinitionName Reader -ResourceGroupName PayInHSM


# ****************
# Links de interés
# ****************

# Uso de Key Vault desde aplicación web
# https://azure.microsoft.com/es-es/documentation/articles/key-vault-use-from-web-application/

# Registro del Almacén de claves de Azure
# https://azure.microsoft.com/es-es/documentation/articles/key-vault-logging/

# *****************
# Datos importantes
# *****************

# Fecha de caducidad del certificado PayInCert de prueba : 12/1/2020 - no es necesario

# *****************
# Cmdlets varios
# *****************

# Elminar aplicación de AAD
# Remove-AzureRmADApplication -ApplicationObjectId

# Eliminación  de Secreto
# Remove-AzureKeyVaultSecret -VaultName $vaultName -Name $secretName
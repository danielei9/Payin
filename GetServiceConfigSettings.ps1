# **********************************************************************************************
# This sample PowerShell gets the settings you'll need for the *.cscfg files
# Modified 21/3/16 by Israel.perez@pay-in.es
# **********************************************************************************************

# **********************************************************************************************
# You MUST set the following values before running this script
# **********************************************************************************************
$vaultName           = 'PayInKeyVault'
$resourceGroupName   = 'PayInHSM'
$applicationName     = 'PayIn'
$storageName         = 'payinstorage' #Required: texto en minúsculas
$pathToCertFile      = ''
$administratorID	 = '' #Paste ObjectId here - admin@pay-in.es AD Admin
$keyName			 = 'RSAPayInKey'
$key2Name			 = 'RSAMobileKey'

# **********************************************************************************************
# You MAY set the following values before running this script
# **********************************************************************************************
$location            = 'West Europe'                          # Get-AzureLocation                  
$keyFPath			 = '' # Cert to import key RSA  

# **********************************************************************************************
# Should we bounce this script execution?
# **********************************************************************************************
if (($vaultName -eq 'MyVaultName') -or `
    ($resourceGroupName -eq 'MyResourceGroupName') -or `
	($applicationName -eq 'MyAppName') -or `
	($storageName -eq 'MyStorageName') -or `
	($pathToCertFile -eq 'C:\path\Certificate.cer') -or `
	($administratorID -eq '') -or `
	($keyName -eq ''))#-or `
	#($key2Name -eq ''))

{
	Write-Host 'You must edit the values at the top of this script before executing' -foregroundcolor Yellow
	exit
}

if (($pathToCertFile -ne '') -and (-not (Test-Path $pathToCertFile)))
{
	Write-Host 'No certificate file found at '$pathToCertFile -foregroundcolor Yellow
	exit
}
# **********************************************************************************************
# Add Azure Account
# **********************************************************************************************
$AzureSubscription = Get-AzureSubscription
if(-not $AzureSubscription)
{
    Write-Host 'Please add an Azure Subscription'
    Add-AzureAccount
}
# **********************************************************************************************
# Log into Azure
# **********************************************************************************************
Write-Host 'Please log into Azure Resource Manager now' -foregroundcolor Green
Login-AzureRmAccount
$VerbosePreference = "SilentlyContinue"

# **********************************************************************************************
# Prep the cert credential data  
# **********************************************************************************************
if($pathToCertFile -ne '')
{
    Write-Host "Verifying certificate"
    $x509 = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2
    $x509.Import($pathToCertFile)
    $myCertThumbprint = $x509.Thumbprint

    $credValue = [System.Convert]::ToBase64String($x509.GetRawCertData())    
}
$now = [System.DateTime]::Now
$threeYearsFromNow = $now.AddYears(3)

# **********************************************************************************************
# Generates a secure 32-byte symmetric key for authentication
# **********************************************************************************************
Function GenerateSymmetricKey()
{
    Write-Host "Generating a new Password for AAD Application"
    $key = New-Object byte[](32)
    $rng = [System.Security.Cryptography.RNGCryptoServiceProvider]::Create()
    $rng.GetBytes($key)
    return [System.Convert]::ToBase64String($key)
}

# **********************************************************************************************
# Create the resource group and vault if needed
# **********************************************************************************************
$rg = Get-AzureRmResourceGroup -Name $resourceGroupName -ErrorAction SilentlyContinue
if (-not $rg)
{
    New-AzureRmResourceGroup -Name $resourceGroupName -Location $location
}
else
{
    Write-Host "Resource group $resourceGroupName exists!"
}

$vault = Get-AzureRmKeyVault -VaultName $vaultName -ErrorAction SilentlyContinue
if (-not $vault)
{
    Write-Host "Creating vault $vaultName"
    $vault = New-AzureRmKeyVault -VaultName $vaultName `
                             -ResourceGroupName $resourceGroupName `
                             -Sku premium `
                             -Location $location
}
else
{
   Write-Host "The Key vAULT $vaultName exists!"
}

# ***********************
# Keys creation
# ***********************
$keyExists = Get-AzureKeyVaultKey -Name $keyName -VaultNAme $vaultName
if(-not $keyExists)	{							  
    Write-Host "Setting key $keyName in vault $vaultName using Software destination"
    $key = Add-AzureKeyVaultKey -VaultName $vaultName -Name $keyName -Destination Software
}
else
{
   Write-Host "The Key $keyName in vault $vaultName exists!"
}

$key2Exists = Get-AzureKeyVaultKey -Name $key2Name -VaultNAme $vaultName
if((-not $key2Exists) -and $keyFPath -and $key2Name)
{
	Write-Host "Setting key2 $key2Name in vault $vaultName using file path"
    $Password = ConvertTo-SecureString -String 'Pa$$w0rd' -AsPlainText -Force
    $key2 = Add-AzureKeyVaultKey -VaultName $vaultName -Name $key2Name -KeyFilePath $keyFPath -KeyFilePassword $Password
}
else
{
   Write-Host "The Key $keyName in vault $vaultName exists!"
} 

# **********************************************************************************************
# Print the XML settings that should be copied into the CSCFG files
# **********************************************************************************************
Write-Host "Place the following into both CSCFG files for the SampleAzureWebService project:" -ForegroundColor Cyan
#'<App name="StorageAccountName" value="' + $storageName + '" />'
#'<App name="EIGE01ASecretUri" value="' + $secret01A.Id.Substring(0, $secret01A.Id.LastIndexOf('/')) + '" />'
#'<App name="KeyVaultSecretCacheDefaultTimeSpan" value="00:00:00" />'
'<App name="KeyUrl" value="' + $key.Id.Substring(0, $key.Id.LastIndexOf('/')) + '" />'
'<App name="Key2Url" value="' + $key2.Id.Substring(0, $key2.Id.LastIndexOf('/')) + '" />'
#'<App name="StorageAccountKey2Url" value="' + $key2.Id.Substring(0, $key2.Id.LastIndexOf('/')) + '" />'
#'<App name="KeyVaultAuthClientId" value="' + $servicePrincipal.ApplicationId + '" />'
#'<App name="KeyVaultAuthCertThumbprint" value="' + $myCertThumbprint + '" />'
#'<Certificate name="KeyVaultAuthCert" thumbprint="' + $myCertThumbprint + '" thumbprintAlgorithm="sha1" />'
Write-Host

# **********************************************************************************************
# Print out in a file
# **********************************************************************************************
$outputFile = '************************************************************************************' + "`r`n" +
    'Time = ' + [System.DateTime]::Now + "`r`n" +
    "Place the following into both CSCFG files for the SampleAzureWebService project:" + "`r`n" +
    #'<App name="StorageAccountName" value="' + $storageName + '" />'
    #'<App name="EIGE01ASecretUri" value="' + $secret01A.Id.Substring(0, $secret01A.Id.LastIndexOf('/')) + '" />' + "`r`n" +
    #'<App name="KeyVaultSecretCacheDefaultTimeSpan" value="00:00:00" />' + "`r`n" +
    '<App name="KeyUrl" value="' + $key.Id.Substring(0, $key.Id.LastIndexOf('/')) + '" />' + "`r`n" +
    '<App name="Key2Url" value="' + $key2.Id.Substring(0, $key2.Id.LastIndexOf('/')) + '" />' + "`r`n" +
    #'<App name="StorageAccountKey2Url" value="' + $key2.Id.Substring(0, $key2.Id.LastIndexOf('/')) + '" />' + "`r`n" +
    #'<App name="KeyVaultAuthClientId" value="' + $servicePrincipal.ApplicationId + '" />' + "`r`n" +
    '************************************************************************************'
Out-File -FilePath C:\ConfigurationKeyVaultProject.txt -width 120 -Append -InputObject $outputFile
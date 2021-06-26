# ************************
# Mobile key import
# ************************

$suscriptionName = 'Production'
$resourceName    = 'PayInHSM'
$vaultName       = 'PayInKeyVault'
$mobileKeyName   = 'RSAMobileKeyFgv'
$mobileKeyFPath  = 'C:\repos\payin\com_payin_fgv.pfx'
$password        = '' # Password to the import key file
$outputFilePath  = 'C:\respos\payin\MobileKeyImport.log'


# **********************************************************************************************
# Add Azure Account
# **********************************************************************************************
$AzureSubscription = Get-AzureSubscription -Verbose
if(-not $AzureSubscription)
{
    Write-Host 'Please add an Azure Subscription'
    Add-AzureAccount -Verbose
}

# **********************************************************************************************
# Log into Azure
# **********************************************************************************************
Write-Host 'Please log into Azure Resource Manager now' -foregroundcolor Green
Login-AzureRmAccount -SubscriptionName $suscriptionName -Verbose

# ***********************
# Key creation
# **********************

$keyVaultExists = Get-AzureRmKeyVault -VaultName $vaultName -ResourceGroupName $resourceName -Verbose
Write-Host "Verifying key Vault"

if(-not $keyVaultExists)
{
    Write-Host "Error, Key vault don't exists!" -ForegroundColor Red
    exit
}

Write-Host "Key Vault $keyVaultExists exists, creating keys" -ForegroundColor Green
$old_ErrorActionPreference = $ErrorActionPreference
$ErrorActionPreference = "SilentlyContinue"

Write-Host "Verifying if mobile Key exists"
$mobileKeyExists = Get-AzureKeyVaultKey -VaultName $vaultName -Name $mobileKeyName -Verbose
if($mobileKeyExists)
{
    Write-Host "Error verifiyng mobile key!" -ForegroundColor Red
    exit
}
Write-Host "Mobile Key don't exists, setting key2 $mobileKeyName in vault $vaultName using file path" -ForegroundColor Green
         
$fileExists = Test-Path $mobileKeyFPath -Verbose
if(-not $fileExists)
{
    Write-Host "File isn't in the file path!" -ForegroundColor Yellow
    exit
}

# Add Key
$passwordSecured = ConvertTo-SecureString -String $password -AsPlainText -Force -Verbose
$mobilekey = Add-AzureKeyVaultKey -VaultName $vaultName -Name $mobileKeyName -KeyFilePassword $passwordSecured -KeyFilePath $mobileKeyFPath -Verbose

# Check key added
$mobileKeyExists = Get-AzureKeyVaultKey -VaultName $vaultName -Name $mobileKeyName -Verbose
if(-not $mobileKeyExists)
{
    Write-Host "Error verifiyng mobile key added!" -ForegroundColor Red
    exit
}
Write-Host "Mobile Key already added, setting key2 $mobileKeyName in vault $vaultName using file path" -ForegroundColor Green

# Summarize
Write-Host $mobilekey.Id.Substring(0, $mobilekey.Id.LastIndexOf('/')) -foregroundcolor Green
    $outputFile = $mobilekey.Id.Substring(0, $mobilekey.Id.LastIndexOf('/')) + "`r`n"
    Out-File -FilePath $outputFilePath -width 120 -Append -InputObject $outputFile

$ErrorActionPreference = $old_ErrorActionPreference

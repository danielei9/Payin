# ************************
# Secrets creation
# ************************


# **********************************************************************************************
# Add Azure Account
# **********************************************************************************************
$AzureSubscription = Get-AzureSubscription
Login-AzureRmAccount

$vaultName           = 'PayInKeyVault'

$secretListName = "EIGEKeysJson"
$secretList = Get-Content -Path C:\EigeKeysJson.json |Out-String
if(-not $secretList)
{
    Write-Host "EIGE keys file not exists!"
    exit
}
$secretExists = Get-AzureKeyVaultSecret -VaultName $vaultName -Name $secretListName
if(-not $secretExists)
{


    $secret = Set-AzureKeyVaultSecret -VaultName $vaultName `
    								  -Name $secretListName `
    								  -SecretValue (ConvertTo-SecureString -String $secretList -AsPlainText -Force)
                 Write-Host $secret.Id.Substring(0, $secret.Id.LastIndexOf('/')) -foregroundcolor Green
                 $outputFile = $secret.Id.Substring(0, $secret.Id.LastIndexOf('/')) + "`r`n"
                 Out-File -FilePath C:\SecretUris.txt -width 120 -Append -InputObject $outputFile 
}
else 
{
    Write-Host "Secret $secretListName exists!"
	
	# Remove-AzureKeyVaultSecret -VaultName $vaultName -Name $secretListName
	# Write-Host "Secret $secretListName deleted"
}

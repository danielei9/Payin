# ************************
# Create Key Vault
# ************************

$locationName    = 'West Europe'
$suscriptionName = 'BizSpark'
$resourceName    = 'PayIn2'
$vaultName       = 'PayIn'
$vaultADClientId = '8da8dc92-725a-4efb-9ecf-59b2e24b6ef0'

# Login
Login-AzureRmAccount -SubscriptionName $suscriptionName -Verbose

# Create ResourGroup
New-AzureRmResourceGroup –Name $resourceName –Location $locationName -Verbose

# Create KeyVault
New-AzureRmKeyVault -VaultName $vaultName -ResourceGroupName $resourceName -Location $locationName -Verbose

# Add KeyVault to ActiveDirectory
Set-AzureRmKeyVaultAccessPolicy -VaultName $vaultName -ServicePrincipalName $vaultADClientId -PermissionsToKeys encrypt,decrypt,sign -PermissionsToSecrets Get -Verbose

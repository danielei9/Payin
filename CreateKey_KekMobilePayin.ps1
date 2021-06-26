# ************************
# Create key
# ************************

$suscriptionName = 'BizSpark'
$vaultName       = 'PayIn'
$mobileKeyName   = 'KekMobilePayin'
$mobileKeyFPath  = 'C:\repos\payin\com_payin.pfx'
$password        = '' # Password to the import key file

# Login
Login-AzureRmAccount -SubscriptionName $suscriptionName -Verbose

# Add Key
$passwordSecured = ConvertTo-SecureString -String $password -AsPlainText -Force -Verbose
Add-AzureKeyVaultKey -VaultName $vaultName -Name $mobileKeyName -KeyFilePassword $passwordSecured -KeyFilePath $mobileKeyFPath -Verbose
Get-AzureKeyVaultKey -VaultName $vaultName -Name $mobileKeyName
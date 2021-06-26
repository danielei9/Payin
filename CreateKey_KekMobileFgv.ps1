# ************************
# Create key
# ************************

$suscriptionName = 'BizSpark'
$vaultName       = 'PayIn'
$mobileKeyName   = 'KekMobileFgv'
$mobileKeyFPath  = 'C:\repos\payin\com_payin_fgv.pfx'
$password        = '' # Password to the import key file

# Login
Login-AzureRmAccount -SubscriptionName $suscriptionName -Verbose

# Add Key
$passwordSecured = ConvertTo-SecureString -String $password -AsPlainText -Force -Verbose
Add-AzureKeyVaultKey -VaultName $vaultName -Name $mobileKeyName -KeyFilePassword $passwordSecured -KeyFilePath $mobileKeyFPath -Verbose
Get-AzureKeyVaultKey -VaultName $vaultName -Name $mobileKeyName

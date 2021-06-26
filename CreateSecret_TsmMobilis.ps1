# ************************
# Create secret
# ************************

$suscriptionName = 'BizSpark'
$vaultName       = 'PayIn'
$mobileKeyName   = 'SecretTsmMobilis'
$mobileSecretFPath = 'C:\repos\payin\SecretTsmMobilis.json'

# Login
Login-AzureRmAccount -SubscriptionName $suscriptionName -Verbose

# Load secret
$secret = Get-Content -Path $mobileSecretFPath | Out-String -Verbose

# Add Secret
$passwordSecured = ConvertTo-SecureString -String $secret -AsPlainText -Force -Verbose
Set-AzureKeyVaultSecret -VaultName $vaultName -Name $mobileKeyName -SecretValue $passwordSecured -Verbose

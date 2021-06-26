Login-AzureRmAccount

$vaultName           = 'PayInKeyVault'

$secret00AName       = 'EIGE00A' 
$secret01AName       = 'EIGE01A' 
$secret02AName       = 'EIGE02A' 
$secret03AName       = 'EIGE03A'
$secret04AName       = 'EIGE04A'
$secret05AName       = 'EIGE05A'
$secret06AName       = 'EIGE06A'
$secret07AName       = 'EIGE07A'
$secret08AName       = 'EIGE08A'
$secret09AName       = 'EIGE09A'
$secret10AName       = 'EIGE10A'
$secret11AName       = 'EIGE11A'
$secret12AName       = 'EIGE12A'
$secret13AName       = 'EIGE13A'
$secret14AName       = 'EIGE14A'
$secret15AName       = 'EIGE15A'
$secret00BName       = 'EIGE00B'
$secret01BName       = 'EIGE01B'
$secret02BName       = 'EIGE02B'
$secret03BName       = 'EIGE03B'
$secret04BName       = 'EIGE04B'
$secret05BName       = 'EIGE05B'
$secret06BName       = 'EIGE06B'
$secret07BName       = 'EIGE07B'
$secret08BName       = 'EIGE08B'
$secret09BName       = 'EIGE09B'
$secret10BName       = 'EIGE10B'
$secret11BName       = 'EIGE11B'
$secret12BName       = 'EIGE12B'
$secret13BName       = 'EIGE13B'
$secret14BName       = 'EIGE14B'
$secret15BName       = 'EIGE15B'
$secretListName      = 'EIGEKeysJson'

$secretsArray = @(
    ( $secret00AName ),
    ( $secret01AName ),
    ( $secret02AName ),
    ( $secret03AName ),
    ( $secret04AName ),
    ( $secret05AName ),
    ( $secret06AName ),
    ( $secret07AName ),
    ( $secret08AName ),
    ( $secret09AName ),
    ( $secret10AName ),
    ( $secret11AName ),
    ( $secret12AName ),
    ( $secret13AName ),
    ( $secret14AName ),
    ( $secret15AName ),
    ( $secret00BName ),
    ( $secret01BName ),
    ( $secret02BName ),
    ( $secret03BName ),
    ( $secret04BName ),
    ( $secret05BName ),
    ( $secret06BName ),
    ( $secret07BName ),
    ( $secret08BName ),
    ( $secret09BName ),
    ( $secret10BName ),
    ( $secret11BName ),
    ( $secret12BName ),
    ( $secret13BName ),
    ( $secret14BName ),
    ( $secret15BName ),
    ( $secretListName ))

$keyVaultExists = Get-AzureRmKeyVault -VaultName PayInKeyVault
Write-Host "Verifying key Vault"
if($keyVaultExists)
{
 Write-Host "Key Vault $keyVaultExists exists, deleting EIGE keys"
    $old_ErrorActionPreference = $ErrorActionPreference
    $ErrorActionPreference = "SilentlyContinue"

     foreach($value in $secretsArray)
    {
         $secretExists = Get-AzureKeyVaultSecret -Name $value -VaultName $vaultName
         Write-Host "el secreto existe"
         if($secretExists)
         {
            Remove-AzureKeyVaultSecret  -VaultName $vaultName -Name $value
            $secretExists = Get-AzureKeyVaultSecret -Name $value -VaultName $vaultName
            if(-not $secretExists)
            {
                 Write-Host "Secret $value deleted" -foregroundcolor Green
            }
            else
            {
                Write-Host "Erase error in  $value" -foregroundcolor Red
            }
         }
         else
         {
            Write-Host "Secret $value not exists "-foregroundcolor Yellow
         }
    }
    $ErrorActionPreference = $old_ErrorActionPreference
}
Login-AzureRmAccount

#Variables
$publicIPName = 'PayInPublicIP'
$location = 'North Europe' #For public IP reservation
$ReservedIP = Get-AzureReservedIP


#Public IP reservation
if(-not $ReservedIP)
{
    Write-Host "Generating reserved IP"
    $publicIP = New-AzureReservedIP –ReservedIPName $publicIPName –Location $location
    Write-Host "Generation Done"

    $publicIPGet = Get-AzureReservedIP –ReservedIPName $publicIPName
    Write-Host "The new public IP is: " $publicIPGet.Address    
}
else
{
    Write-Host "Public IPs already exists. Up to five Public IPs carry an additional charge. Review it."
}

#Eliminar IP reservada
#Remove-AzureReservedIP -ReservedIPName $publicIPName
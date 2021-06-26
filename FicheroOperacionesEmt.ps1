################################
# Script Blob to FTP EMT files #
################################

#begin


# Update with the name of your subscription.
$subscriptionName = "admin@pay-in.es"
$subscriptionId = "fcd052c1-bd15-4355-b962-f9d579ddfd61"

# Give a name to your new storage account. It must be lowercase!
$storageAccountName = "payinstorageaccount"
$resourceGroupName = "payintemplatetest"

# Choose "West US" as an example.
$Location = "North Europe"

# Give a name to your new container.
$containerName = "emtfiles"

# Have an image file and a source directory in your local computer.
$ImageToUpload = "C:\20161003.txt"

# Date operation and file name operations
$yesterdayDate = (Get-Date).AddDays(-1)
$dateFormat = $yesterdayDate.ToString("yyyyMMdd")
$emtFileName = $dateFormat + ".txt"

#Blob Key
$storageAccountKey = "VK/93NkkLqQoGwBUUVcFUZWUjqBgY9HdJBP0Fo+SJi14wctfmg4Ksy821gk03idYAVnk0j/6YaZIjQImjp1/hw=="

#FTP credential
$username = 'prueba2ftp\$prueba2ftp'
$password = 'Gbuya38r7TXafCX0Ba8SFx4a79HoWrG8eYbXXv04K78ur7il25l1b3loqSb5'
$ftpUrl = 'ftp://waws-prod-sn1-053.ftp.azurewebsites.windows.net/site/wwwroot/emtfiles/'

##########
# Script #
##########

# Add your Azure account to the local PowerShell environment.
Add-AzureAccount

# Set a default Azure subscription.
Select-AzureSubscription -SubscriptionName $subscriptionId –Default

        
# Obtener clave del storage 
# $storageAccountKey = (Get-AzureRmStorageAccountKey -ResourceGroupName $resourceGroupName -Name $storageAccountName)[0].key1

# Azure storage context
 $context = New-AzureStorageContext -StorageAccountName $storageAccountName -StorageAccountKey $storageAccountKey

# Create a new storage account.
# $newStorageAccount = New-AzureStorageAccount –StorageAccountName $storageAccountName -Location $Location 

# Set a default storage account.
# Set-AzureSubscription Get-CurrentStorageAccountName $storageAccountName -SubscriptionId $subscriptionId

# Create a new container.
#$ContainerExists = (Get-AzureStorageContainer  -Name 'emtfiles' -Context $context )
#if (!$ContainerExists)
#{
#    New-AzureStorageContainer -Name $containerName -Permission Off
#}
# Upload a blob into a container.
# Set-AzureStorageBlobContent -Container $containerName -File $ImageToUpload -Context $context

# List all blobs in a container.
Get-AzureStorageBlob -Container $containerName -Context $context

# Download blobs from the container:
# Get a reference to a list of all blobs in a container.

 $blob = Get-AzureStorageBlob  -Container $containerName -Blob $emtFileName -Context $context 
 $cloudBlockBlob = [Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob] $blob.ICloudBlob

# $content = Get-AzureStorageBlob -Container $containerName -Blob $emtFileName -Context $context | Get-AzureStorageBlobContent -Destination C:/

if($blob)
{
    $webclient = New-Object System.Net.WebClient  
    $webclient.Credentials = New-Object System.Net.NetworkCredential($username,$password)  
    $webclient2 = New-Object System.Net.WebClient 

    #list every sql server trace file     
    $fileBytes = $webclient2.DownloadData($cloudBlockBlob.Uri.AbsoluteUri)
    $uri = New-Object System.Uri($ftpUrl+$emtFileName) 
    $webclient.UploadData($uri, $fileBytes) 
}


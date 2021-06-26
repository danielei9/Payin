################################
# Script Blob to FTP EMT files #
################################

# Variables #
#===========#
$subscriptionId = "fcd052c1-bd15-4355-b962-f9d579ddfd61"
$storageAccountName = "payinstorageaccount" # in lowercase
$resourceGroupName = "payintemplatetest"
$Location = "North Europe"

# Give a name to your new container.
$containerName = "emtfiles"

# Date operation and file name operations
$yesterdayDate = (Get-Date).AddDays(-1)
$dateFormat = $yesterdayDate.ToString("yyyyMMdd")
$emtFileName = $dateFormat + ".txt"

#Blob Key
$storageAccountKey = "VK/93NkkLqQoGwBUUVcFUZWUjqBgY9HdJBP0Fo+SJi14wctfmg4Ksy821gk03idYAVnk0j/6YaZIjQImjp1/hw=="

#FTP credentials
$username = 'prueba2ftp\$prueba2ftp'
$password = 'Gbuya38r7TXafCX0Ba8SFx4a79HoWrG8eYbXXv04K78ur7il25l1b3loqSb5'
$ftpUrl = 'ftp://waws-prod-sn1-053.ftp.azurewebsites.windows.net/site/wwwroot/emtfiles/'
#$progressPreference = "silentlyContinue" #Para evitar desbordamiento de buffer silenciando outputs
##########
# Script #
##########


# Azure storage context
$context = New-AzureStorageContext -StorageAccountName $storageAccountName -Anonymous #-StorageAccountKey $storageAccountKey 
#$sasToken = New-AzureStorageContainerSASToken -Container $containerName -Permission rl -Context $context
#$contextSasToken = New-AzureStorageContext -StorageAccountName $storageAccountName -SasToken $sasToken

# Download blobs from the container
$progressPreference = "silentlyContinue"
$blob = Get-AzureStorageBlob -Container $containerName -Blob $emtFileName -Context $context 
$progressPreference = "Continue"
$cloudBlockBlob = [Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob] $blob.ICloudBlob

# Download from StorageAccount and Upload to FTP server
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


using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Xp.Infrastructure.Repositories
{
    public class AzureBlobRepository : IBlobRepository
    {
        #region SaveFile
        public string SaveFile(string name, byte[] content)
        {
            var containerName = ConfigurationManager.ConnectionStrings["Files"];
            var storageAccount = CloudStorageAccount.Parse(containerName.ConnectionString);

            // Create the blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            var container = blobClient.GetContainerReference("files");
            var blockBlob = container.GetBlockBlobReference(name);

            if (blockBlob.Exists() && content != null)
                DeleteFile(name);

            blockBlob.UploadFromByteArray(content, 0, content.Length);

            return blockBlob.StorageUri.PrimaryUri.AbsoluteUri;
        }
        public string SaveFile(string name, string content)
        {
            var containerName = ConfigurationManager.ConnectionStrings["Files"];
            var storageAccount = CloudStorageAccount.Parse(containerName.ConnectionString);

            // Create the blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            var container = blobClient.GetContainerReference("files");
            var blockBlob = container.GetBlockBlobReference(name);

            if (blockBlob.Exists() && !content.IsNullOrEmpty())
                DeleteFile(name);

            blockBlob.UploadText(content, Encoding.UTF8);

            return blockBlob.StorageUri.PrimaryUri.AbsoluteUri;
        }
        #endregion

        #region SaveImage
        public string SaveImage(string name, byte[] content)
        {
            var conn = ConfigurationManager.ConnectionStrings["Files"];
            var storageAccount = CloudStorageAccount.Parse(conn.ConnectionString);

            // Create the blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            var container = blobClient.GetContainerReference("files");

            var blockBlob = container.GetBlockBlobReference(name);

            if (blockBlob.Exists() && content != null)
            {
                DeleteFile(name);

                blockBlob = container.GetBlockBlobReference(name);
            }

            try
            {
                var text = Encoding.UTF8.GetString(content).Split(',')[1];
                byte[] bytes = Convert.FromBase64String(text);

                using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
                {
                    ms.Write(bytes, 0, bytes.Length);
                    Image image = Image.FromStream(ms, true);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        image.Save(memoryStream, ImageFormat.Jpeg);
                        byte[] imageBytes = memoryStream.ToArray();

                        blockBlob.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);
                        blockBlob.Properties.ContentType = "image/jpg";

                        // PRUEBA PARA VER SI EL FICHERO SE HA CREADO BIEN
                        var comprobarSiExiste = blockBlob.Exists();

                        return blockBlob.StorageUri.PrimaryUri.AbsoluteUri;
                    }
                }
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("Binary byte[] of photo is null.");
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message, exc.InnerException);
            }
        }
        #endregion

        #region LoadStringFile
        public async Task<string> LoadStringFileAsync(string name)
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("Files"));

            // Create the blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            var container = blobClient.GetContainerReference("files");
            var blockBlob = container.GetBlockBlobReference(name);

            return await blockBlob.DownloadTextAsync();
        }
        #endregion

        #region LoadStringUrlAsync
        public async Task<string> LoadStringUrlAsync(string url)
        {
            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            var result = await client.DownloadStringTaskAsync(url);
            return result;
        }
        #endregion

        #region DeleteFile
        public void DeleteFile(string name)
        {
            try
            {
                //Retrieve storage account from connection string
                var conn = ConfigurationManager.ConnectionStrings["Files"];
                var storageAccount = CloudStorageAccount.Parse(conn.ConnectionString);

                //Create the blob client
                var blobClient = storageAccount.CreateCloudBlobClient();

                //Retrieve reference to a previously created container
                var container = blobClient.GetContainerReference("files");

                var storageUrl = container.Uri.ToString();
                var lengthShortUrl = storageUrl.Length + 1;
                var shortUrl = "";

                if (name.Contains(storageUrl))
                {
                    shortUrl = name.Substring(lengthShortUrl);
                }
                else shortUrl = name;
                //Retrieve reference to a blob named "myblob.txt"
                var blockBlob = container.GetBlockBlobReference(shortUrl);

                //Delete the blob
                if (blockBlob.Exists())
                    blockBlob.Delete();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message, exc.InnerException);
            }
        }
        #endregion

        //TO DO
        #region FileCreate
        public async Task<bool> FileCreateAsync(string name, string containerName, string entry)
        {
            try
            {
                //Retrieve storage account from connection string				
                var conn = ConfigurationManager.ConnectionStrings["EmtFiles"];
                var storageAccount = CloudStorageAccount.Parse(conn.ConnectionString);
                //Create the blob client
                var blobClient = storageAccount.CreateCloudBlobClient();

                //Retrieve reference to a previously created container  emtfiles
                var container = blobClient.GetContainerReference(containerName);
                if (container == null) return false;

                var blockBlob = container.GetBlockBlobReference(name + ".txt");
                var exists = await blockBlob.ExistsAsync();

                if (exists)
                {
                    var oldText = "";
                    using (var stream = blockBlob.OpenRead())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            while (!reader.EndOfStream)
                            {
                                oldText += reader.ReadLine() + Environment.NewLine;
                            }
                        }
                    }
                    oldText += entry;
                    blockBlob.UploadText(oldText);
                    return true;
                }
                else
                {
                    string nameBlob = name + ".txt";
                    CloudBlockBlob blob = container.GetBlockBlobReference(nameBlob);
                    blob.UploadText(entry);
                    return false;
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message, exc.InnerException);
            }
        }

        public Task<bool> FileExistsAsync(string name, string containerName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

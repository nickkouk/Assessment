using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Assessment.Repositories
{
    /// <summary>
    /// A storage service responsible for storing and deleting photos to Azure blob Storage 
    /// </summary>
    public class AzureStorage:IStorageService
    {
        const string STORAGE_KEY = "StorageConnectionString";
        private CloudBlobContainer container;

        //Constructor retieves the container
        public AzureStorage()
        {
            try
            {
                //Use CloudStorageAccount.parse static method to retrieve the account. Credentials are stored at web.config App settings
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings[STORAGE_KEY].ToString());
                // Create a blob client and retrieve reference to images container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                container = blobClient.GetContainerReference("images");

                //The first time the application runs container doesn't exist
                // Create the "images" container if it doesn't already exist.
                if (container.CreateIfNotExists())
                {
                    // Enable public access on the newly created "images" container
                    container.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess =
                                BlobContainerPublicAccessType.Blob
                        });
                }
            }
            catch (Exception exc)
            {
                //To Do: Create a logger service
            }
        }

        /// <summary>
        /// Adds the <paramref name="postedImage"/> to Azure Blob 
        /// and returns the path
        /// </summary>
        public string CreateImage(HttpPostedFileBase postedImage)
        {
            string photoPath = String.Empty;
            if (postedImage == null || postedImage.ContentLength == 0)
            {
                return null;
            }
            try
            {
                // Create a unique name for the image we are about to store
                string imageName = String.Format("photo_{0}_{1}",
                    Guid.NewGuid().ToString(),
                    Path.GetExtension(postedImage.FileName));

                // Upload image to Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);
                blockBlob.Properties.ContentType = postedImage.ContentType;
                blockBlob.UploadFromStream(postedImage.InputStream);
                photoPath = blockBlob.Uri.ToString();
            }
            catch (Exception exc)
            {

            }
            return photoPath;
        }

        /// <summary>
        /// Deletes the Image located at the <paramref name="uri"/> from the blob storage
        /// </summary>
        public void DeleteImage(string uri)
        {
            try
            {
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(uri);
                blockBlob.DeleteIfExists();
            }
            catch(Exception exc)
            {

            }
        }
    }
}
﻿using Assessment.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Assessment.Repositories
{
    /// <summary>
    /// A storage service responsible for storing and deleting photos to Azure blob Storage 
    /// </summary>
    public class AzureStorage : IStorageService
    {
        const string STORAGE_KEY = "StorageConnectionString";
        private CloudBlobContainer container;
        private ILogger logger;
        //Constructor injects the logger and retieves the blob container
        public AzureStorage(ILogger logger)
        {
            this.logger = logger;
            try
            {
                //Use CloudStorageAccount.parse static method to retrieve the account. Credentials are stored at web.config App settings
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings[STORAGE_KEY].ToString());
                // Create a blob client and retrieve reference to images container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                container = blobClient.GetContainerReference("images");
                logger.Information("Successfully retrieved container");
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
                    logger.Warning("Container:{0} Created", container.Name);
                }
            }
            catch (Exception exc)
            {
                logger.Error(exc, "Failed To retrieve container");
                throw;
            }
        }

        /// <summary>
        /// Adds the <paramref name="postedImage"/> to Azure Blob 
        /// and returns the path
        /// </summary>
        public string CreateImage(HttpPostedFileBase postedImage)
        {
            //create a stopwatch for every image upload to calculate time elapsed.
            Stopwatch timespan = Stopwatch.StartNew();
            string photoPath = String.Empty;

            if (postedImage == null || postedImage.ContentLength == 0)
            {
                logger.Warning("Couldn't upload image to Azure Blob Storage. Posted file was empty.");
                return null;
            }
            try
            {
                // Create a unique name for the image we are about to store
                string imageName = String.Format("photo_{0}{1}",
                     Guid.NewGuid().ToString(),
                    Path.GetExtension(postedImage.FileName));
                // Upload image to Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);
                blockBlob.Properties.ContentType = postedImage.ContentType;
                blockBlob.UploadFromStream(postedImage.InputStream);
                //the path will be returned and stored to database
                photoPath = blockBlob.Uri.ToString();
                //Trace the time elapsed
                timespan.Stop();
                logger.TraceApi("Azure Blob Storage", "AzureStorage.CreateImage", timespan.Elapsed, "Image:{0} uploaded to Azure Blob Storage.", imageName);
                Image thumbnail=CreateImageThumbnail(postedImage, imageName);
            }
            catch (Exception exc)
            {
                logger.Error(exc, "failed to upload image to Azure Blob Storage");
                throw;
            }
            return photoPath;
        }

        /// <summary>
        /// Creates an image thumbnail for the <param name="postedImage"/> and stores it to 
        /// Azure Blob Storage
        /// </summary>
        private Image CreateImageThumbnail(HttpPostedFileBase postedImage,string path)
        {
            //The thumb has the same unique name like image but is prefixed with thumb
            var thumbPath = path.Replace("photo_","thumb_");
            Image thumb=null;
            try
            {
                int thumbHeight = 200;
                int thumbWidth = 200;
                Image image = Image.FromStream(postedImage.InputStream, true, true);
                if (image.Width < image.Height)
                {
                    thumbWidth = thumbHeight * image.Width / image.Height;
                }
                else if (image.Width > image.Height)
                {
                    thumbHeight = thumbWidth * image.Height / image.Width;
                }
                
                thumb= image.GetThumbnailImage(thumbWidth, thumbHeight, () => false, IntPtr.Zero);
                using (var ms = new MemoryStream())
                {
                    thumb.Save(ms,image.RawFormat);
                    ms.Position = 0;
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(thumbPath);
                    blockBlob.Properties.ContentType = postedImage.ContentType;

                    blockBlob.UploadFromStream(ms);
                }
                

                
            }
            catch(Exception exc)
            {
                logger.Error(exc, "Failed to Create image thumbnail");
                throw;
            }
            return thumb;
        }


        /// <summary>
        /// Deletes the Image located at the <paramref name="uri"/> from the blob storage
        /// </summary>
        public void DeleteImage(string uri)
        {
            try
            {
                string imageUri = Path.GetFileName(uri);
                string thumbUri = imageUri.Replace("photo_", "thumb_");
                CloudBlockBlob imageBlogBlob = container.GetBlockBlobReference(imageUri);
                imageBlogBlob.DeleteIfExists();
                CloudBlockBlob thumbBlockBlob = container.GetBlockBlobReference(thumbUri);
                thumbBlockBlob.DeleteIfExists();
                logger.Information("Successfully deleted image from Azure Blob Storage at URI:{0}", uri);
            }
            catch (Exception exc)
            {
                logger.Error(exc, "Failed to delete image from Azure Blob Storage at URI:{0}", uri);
                throw;
            }
        }


    }
}
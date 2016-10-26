using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Assessment.Models;
using Assessment.Logging;
using System.Diagnostics;

namespace Assessment.Repositories
{
    public class ImageRepository : IImagesService
    {
        private ImagesDbContext context = new ImagesDbContext();
        private IStorageService storageService;
        private ILogger logger;

        //Inject a storage service and a logger from the constructor
        public ImageRepository(IStorageService storageService,ILogger logger)
        {
            this.storageService = storageService;
            this.logger = logger;
        }
        /// <summary>
        /// Returns all images 
        /// </summary>
        /// <returns></returns>
        public List<Image> GetImages()
        {
            return context.Images.ToList();
        }

        /// <summary>
        /// Adds the supplied <paramref name="image"/> to the system and returns the Id.
        /// Part of the operation is to store the Image in the blob storage.
        /// *This method will have to change,so it will also take a photo parameter
        /// </summary>
        public int AddNewImage(Image image,HttpPostedFileBase postedImage)
        {
            try
            {
                //Call a service to upload the image and store the path to the database entry.
                image.ImagePath = storageService.CreateImage(postedImage);
                //Start a stopwatch after the image is uploaded in order to Trace the database transaction time elapsed
                Stopwatch timespan = Stopwatch.StartNew();

                context.Images.Add(image);
                context.SaveChanges();

                timespan.Stop();
                logger.TraceApi("SQL Database", "ImageRepository.AddNewImage", timespan.Elapsed,"Successfully added image to database.");
            }
            catch (Exception exc)
            {
                logger.Error(exc, "failed to add Image to Database");
                throw;
            }
            return image.Id;
        }

        /// <summary>
        /// Deletes the Image with the supplied <paramref name="id"/> from the system 
        /// and deletes the file from the blob storage as well.
        /// </summary>
        public void DeleteImage(int id)
        {
            try
            {
                Image entry = context.Images.Find(id);
                if (entry != null)
                {
                    if (entry.ImagePath != null)
                    {
                        storageService.DeleteImage(entry.ImagePath);
                    }
                    context.Images.Remove(entry);
                    
                    context.SaveChanges();
                    logger.Information("Successfully deleted image:{0} from Database",id);
                   
                }
            }
            catch(Exception exc)
            {
                logger.Error(exc, "Failed to delete image:{0} from Database",id);
                throw;
            }
           
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Assessment.Models;

namespace Assessment.Repositories
{
    public class ImageRepository : IImagesService
    {
        private ImagesDbContext context = new ImagesDbContext();
        private IStorageService storageService;

        //Inject a storage service from the constructor
        public ImageRepository(IStorageService storageService)
        {
            this.storageService = storageService;
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
            //set the image path to the uploaded file's path
            image.ImagePath=storageService.CreateImage(postedImage);
            context.Images.Add(image);
            context.SaveChanges();
            return image.Id;
        }

        /// <summary>
        /// Deletes the Image with the supplied <paramref name="id"/> from the system 
        /// and deletes the file from the blob storage as well.
        /// </summary>
        public void DeleteImage(int id)
        {
            Image entry = context.Images.Find(id);
            if (entry != null)
            {
                if(entry.ImagePath!=null)
                {
                    storageService.DeleteImage(entry.ImagePath);
                }
                context.Images.Remove(entry);
                context.SaveChanges();
            }
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assessment.Repositories
{
    /// <summary>
    /// Service responsible for storing/deleting images 
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Uploads the image to a storage and returns the path
        /// </summary>
        /// <returns></returns>
        string CreateImage(HttpPostedFileBase postedImage);

        /// <summary>
        /// Deletes image from the storage
        /// </summary>
        void DeleteImage(string uri);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assessment.Models
{
    public class Image
    {
        /// <summary>
        /// The Id of the entity
        /// </summary>
        /// 
        public int Id { get; set; }

        /// <summary>
        /// The name of the image. It can be different than actual file name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the image
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The path the actual image is stored (normally the blob storage reference)
        /// </summary>
        public string ImagePath { get; set; }
    }
}
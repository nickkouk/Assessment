using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        [RegularExpression("^[a-zA-Z0-9_]*$", ErrorMessage ="Name should only contain alphanumeric characters and undersocores!")]
        [StringLength(100,MinimumLength =4,ErrorMessage ="Name must be at least 4 characters long!")]
        public string Name { get; set; }

        /// <summary>
        /// The optional description of the image
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The path the actual image is stored (normally the blob storage reference)
        /// </summary>
        public string ImagePath { get; set; }
    }
}
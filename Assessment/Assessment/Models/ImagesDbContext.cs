using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Assessment.Models
{
    public class ImagesDbContext : DbContext
    {
        //for demonstration reasons i left the connection string to the default localDb
        public ImagesDbContext() : base()
        {
        }
        public DbSet<Image> Images { get; set; }
    }
}
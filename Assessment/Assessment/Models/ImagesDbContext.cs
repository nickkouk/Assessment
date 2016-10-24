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
            //Update the database if model changes. !!Development mode only!!
            Database.SetInitializer<ImagesDbContext>(new DropCreateDatabaseIfModelChanges<ImagesDbContext>());

        }
        public DbSet<Image> Images { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assessment.Models;
using Assessment.Repositories;

namespace Assessment.Controllers
{
    public class ImagesController : Controller
    {
        //This repository will handle all database updates
        //and physical storage of photos in Azure
        private IImagesService imageRepository;

        //The repository is injected in the Controllers constructor
        public ImagesController(IImagesService imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        // GET: Images
        public ActionResult Index()
        {
            return View(imageRepository.GetImages());
        }

        // GET: Images/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = imageRepository.GetImages().Find(im => im.Id == id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // GET: Images/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,ImagePath")] Image image,HttpPostedFileBase postedImage)
        {
            if (ModelState.IsValid)
            {
                imageRepository.AddNewImage(image,postedImage);
                return RedirectToAction("Index");
            }

            return View(image);
        }

        //For the Assesment Edit View wasn't requested
        //// GET: Images/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Image image = db.Images.Find(id);
        //    if (image == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(image);
        //}

        //// POST: Images/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Name,Description,ImagePath")] Image image)
        //{
        //    if (ModelState.IsValid)
        //    {


        //        return RedirectToAction("Index");
        //    }
        //    return View(image);
        //}

        // GET: Images/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = imageRepository.GetImages().Find(im => im.Id == id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            imageRepository.DeleteImage(id);
            return RedirectToAction("Index");
        }

    }
}

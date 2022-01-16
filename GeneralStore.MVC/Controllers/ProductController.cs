using GeneralStore.MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GeneralStore.MVC.Controllers
{
    public class ProductController : Controller
    {
        // Link to the Db with the application DB Context 
        private readonly ApplicationDbContext _db = new ApplicationDbContext(); // Readonly?

        // GET: Product
        public ActionResult Index()
        {
            //List<Product> productList = _db.Products.ToList();
            //List<Product> oderList = productList.OrderBy(prod => prod.Name).ToList();
            //return View(oderList);
            return View(_db.Products.ToList()); // why list?
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]// POST Annotation
        [ValidateAntiForgeryToken]// Specific User 
        public ActionResult Create(Product product)
        {//^Create a product in the database
            if (ModelState.IsValid)
            {//^Add Product to the db and save the changes in the db.
                _db.Products.Add(product);
                _db.SaveChanges();
                //^ After saving the changes return back to list(Index). 
                return RedirectToAction("Index");
            }

            return View(product);
            //^If the required product is not valid then return back to the view with the same model that was given
        }

        public ActionResult Delete(int? id) // (Product product)?
        {//^Get Id from the Db
            if(id == null)
            {//^If id does not exist return bad request code to the user
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Product product = _db.Products.Find(id);
            //^Find the product by it's id in the db
            if (product == null)
            {//^ If Product does not exist return not found to the client
                return HttpNotFound();
            }
            
            return View(product);
            //^If the required product does not exist return back to the view with the same model the client requested
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {//^Delete the Porduct by Id

            Product product = _db.Products.Find(id);
            //^Find the product that the client requested in the db  
            
            _db.Products.Remove(product);
            //^Remove the product that is in the db

            _db.SaveChanges();
            //^Save the changes in the db
            
            return RedirectToAction("Index");
            //^Once completed redirect the client Back to List(Index)
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = _db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(product).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = _db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }
    }
}
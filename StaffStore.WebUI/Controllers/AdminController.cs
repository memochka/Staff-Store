using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaffStore.Domain.Abstract;
using StaffStore.Domain.Entities;
using StaffStore.Domain.Concrete;

namespace StaffStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductsRepository repository;
        private EFDbContext db = new EFDbContext();

        public AdminController(IProductsRepository repo)
        {
            repository = repo;
        }

        //public ViewResult Index()
        //{
        //    return View(repository.Products);
        //}
        public ViewResult Index(string sortOrder, string searchString)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "NameAsc" ? "NameDesc" : "NameAsc";
            ViewBag.PriceSortParm = sortOrder == "PriceAsc" ? "PriceDesc" : "PriceAsc";

            var repository = from s in db.Products
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                repository = repository.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "NameDesc":
                    repository = repository.OrderByDescending(s => s.Name);
                    break;
                case "PriceAsc":
                    repository = repository.OrderBy(s => s.Price);
                    break;
                case "PriceDesc":
                    repository = repository.OrderByDescending(s => s.Price);
                    break;
                default:
                    repository = repository.OrderBy(s => s.Name);
                    break;
            }

            return View(repository.ToList());
        }


        public ViewResult Edit(int productId)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                repository.SaveProduct(product);
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data value
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product deletedProduct = repository.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("{0} was deleted", deletedProduct.Name);
            }
            return RedirectToAction("Index");
        }
    }
}
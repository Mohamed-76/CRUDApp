using CRUDApp.Models;
using CRUDApp.sevices;
using Microsoft.AspNetCore.Mvc;

namespace CRUDApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext appDbContext;
        private readonly IWebHostEnvironment hostEnvironment;

        public ProductsController(AppDbContext appDbContext, IWebHostEnvironment hostEnvironment)
        {
            this.appDbContext = appDbContext;
            this.hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            var products = appDbContext.products.OrderByDescending(p => p.Id).ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            if (productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The Image is Required");
            }
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            fileName += Path.GetExtension(productDto.ImageFile.FileName);
            string fullPath = Path.Combine(hostEnvironment.WebRootPath, "images", fileName);
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                productDto.ImageFile.CopyTo(fileStream);
            }
            Product product = new Product
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                Price = productDto.Price,
                Category = productDto.Category,
                Description = productDto.Description,
                ImageFilePath = fileName,
                CreatedAt = DateTime.Now
            };
            appDbContext.products.Add(product);
            appDbContext.SaveChanges();
            return RedirectToAction("Index", "Products");
        }
        public IActionResult Edit(int id)
        {
            var product = appDbContext.products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            var productDto = new ProductDto
            {
                Name = product.Name,
                Brand = product.Brand,
                Price = product.Price,
                Category = product.Category,
                Description = product.Description
            };
            ViewData["Product Id"] = product.Id;
            ViewData["Image File"] = product.ImageFilePath;
            ViewData["Created At"] = product.CreatedAt.ToString("MM/dd/yyyy");

            return View(productDto);

        }
        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {
            var product = appDbContext.products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            if (!ModelState.IsValid)
            {

                ViewData["Product Id"] = product.Id;
                ViewData["Image File"] = product.ImageFilePath;
                ViewData["Created At"] = product.CreatedAt.ToString("MM/dd/yyyy");
                return View(productDto);

            }
            string fileName = product.ImageFilePath;
            if (productDto.ImageFile != null)
            {
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(productDto.ImageFile.FileName);
                string fullPath = Path.Combine(hostEnvironment.WebRootPath, "images", fileName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    productDto.ImageFile.CopyTo(fileStream);
                }
                string oldFilePath = Path.Combine(hostEnvironment.WebRootPath, "images", product.ImageFilePath);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
            product.Name = productDto.Name;
            product.Brand = productDto.Brand;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Category = productDto.Category;
            product.ImageFilePath = fileName;
            appDbContext.SaveChanges();
            return RedirectToAction("Index", "Products");
        }
        public IActionResult Delete(int id)
        {
            var product = appDbContext.products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            string filePath = Path.Combine(hostEnvironment.WebRootPath, "images", product.ImageFilePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            appDbContext.products.Remove(product);
            appDbContext.SaveChanges();
            return RedirectToAction("Index", "Products");

        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.DataAccess.Repository.IRepository;
using RMS.Models;
using System.Security.Claims;

namespace RMSWeb.Areas.User.Controllers
{
    [Area("User")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly IPurchaseOrderCartRepository _cartRepo;
        public ProductController(IProductRepository productRepo, IPurchaseOrderCartRepository cartRepo)
        {
            _productRepo = productRepo;
            _cartRepo = cartRepo;

        }

        public IActionResult Index()
        {
            List<Product> productList = _productRepo.GetAll().ToList();
            return View(productList);
        }

        public IActionResult Details(int productId)
        {
            PurchaseOrderCart orderCart = new()
            {
                Product = _productRepo.Get(u => u.Id == productId),
                Count = 1,
                ProductId = productId
            };

            return View(orderCart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(PurchaseOrderCart orderCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            orderCart.ApplicationUserId = userId;

            PurchaseOrderCart cartFromDb = _cartRepo.Get(u => u.ApplicationUserId == userId && u.ProductId == orderCart.ProductId);

            if (cartFromDb != null)
            {
                //order cart exists
                cartFromDb.Count += orderCart.Count;
                _cartRepo.Update(cartFromDb);
            }
            else
            {
                //add order in cart
                _cartRepo.Add(orderCart);
            }

            TempData["success"] = "Order cart updated successfully";
            _cartRepo.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}

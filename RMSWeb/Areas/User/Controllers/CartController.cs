using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.DataAccess.Repository.IRepository;
using RMS.Models;
using RMS.Models.ViewModels;
using RMS.Utility;
using RMSWeb.Services.IServices;
using System.Security.Claims;

namespace RMSWeb.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IPurchaseOrderCartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISapService _sapService;

        [BindProperty]
        public PurchaseOrderCartVM PurchaseOrderCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork, IPurchaseOrderCartRepository cartRepository, ISapService sapService)
        {
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
            _sapService = sapService;
        }

        public IActionResult Index()
        {
            //getting UserId
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            PurchaseOrderCartVM = new()
            {
                PurchaseOrderCartList = _cartRepository.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                PurchaseOrderHeader = new()
            };

            return View(PurchaseOrderCartVM);
        }

        public IActionResult OrderSummary()
        {
            //getting UserId
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            PurchaseOrderCartVM = new()
            {
                PurchaseOrderCartList = _cartRepository.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                PurchaseOrderHeader = new()
            };

            PurchaseOrderCartVM.PurchaseOrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            var storeId = PurchaseOrderCartVM.PurchaseOrderHeader.ApplicationUser.StoreId;
            PurchaseOrderCartVM.PurchaseOrderHeader.Store = _unitOfWork.Store.Get(u => u.Id == storeId).Name;

            //foreach (var cart in PurchaseOrderCartVM.PurchaseOrderCartList)
            //{

            //}

            return View(PurchaseOrderCartVM);
        }

        [HttpPost]
        [ActionName("OrderSummary")]
        public IActionResult OrderSummaryPost()
        {
            //getting UserId
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            PurchaseOrderCartVM.PurchaseOrderCartList = _cartRepository.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");

            PurchaseOrderCartVM.PurchaseOrderHeader.ApplicationUserId = userId;

            //if(InquiryAPI is success) - see below method
            PurchaseOrderCartVM.PurchaseOrderHeader.OrderStatus = SD.StatusInProcess;

            _unitOfWork.OrderHeader.Add(PurchaseOrderCartVM.PurchaseOrderHeader);
            _unitOfWork.Save();

            foreach (var cart in PurchaseOrderCartVM.PurchaseOrderCartList)
            {
                PurchaseOrderItem orderItem = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = PurchaseOrderCartVM.PurchaseOrderHeader.Id,
                    Count = cart.Count
                };
                _unitOfWork.OrderItem.Add(orderItem);
                _unitOfWork.Save();
            }


            return RedirectToAction(nameof(OrderConfirmation), new { id = PurchaseOrderCartVM.PurchaseOrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitInquiry(PurchaseOrderCartVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _sapService.PostInquiryRequestAsync<APIResponse>(model);
                if (response != null || response.IsSuccess)
                {
                    //create logic to save it in the database. - need to do.


                    return RedirectToAction(nameof(OrderSummary));
                }
            }
            return View(model);
        }

        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _cartRepository.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _cartRepository.Update(cartFromDb);
            _cartRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _cartRepository.Get(u => u.Id == cartId);
            if (cartFromDb.Count <= 1)
            {
                //remove from cart
                _cartRepository.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _cartRepository.Update(cartFromDb);
            }

            _cartRepository.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _cartRepository.Get(u => u.Id == cartId);

            _cartRepository.Remove(cartFromDb);

            _cartRepository.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}

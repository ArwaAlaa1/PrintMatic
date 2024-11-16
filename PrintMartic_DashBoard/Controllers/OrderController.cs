using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintMartic_DashBoard.Extention;
using PrintMartic_DashBoard.ViewModels;
using PrintMatic.Core;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Entities.Order;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.Services;
using System.IO.Compression;

namespace PrintMartic_DashBoard.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(IUnitOfWork unitOfWork ,IMapper  mapper,IOrderRepository orderRepository,UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes ="Cookies",Roles ="بائع")]
        public async Task<IActionResult> Index(string status = "All")
        {
            OrderStatus parsedStatus;
            var isStatusValid = Enum.TryParse(status, ignoreCase: true, out parsedStatus);

            var user = await _userManager.GetUserAsync(User);
            var allOrders = status == "All"
                ? await _orderRepository.GetOrdersForSpecificCompanyAsync(user.Id).ToListAsync()
                : isStatusValid ? await ( _orderRepository.GetOrdersForSpecificCompanyAsync(user.Id))
                    .Where(o => o.Status== parsedStatus).ToListAsync()
                    : new List<Order>();

            var orders= _mapper.Map< List<Order> ,List <OrderViewModelForCompany>>(allOrders);
            ViewBag.CurrentStatus = parsedStatus.GetEnumMemberValue(); 
            return View(orders);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع")]
        public async Task<IActionResult> OrderDetails(int OrderId)
        {
            var trader=await _userManager.GetUserAsync(User);
           var order=await _orderRepository.GetOrderWithItemsAsync(OrderId,trader.Id);
            var ordermapped = _mapper.Map<Order, OrderViewModelForCompany>(order);
            return View(ordermapped);
        
        }

        
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع")]
        public async void ConfirmOrder(int OrderId)
        {
            var trader = await _userManager.GetUserAsync(User);

           
            var order = await _orderRepository.GetOrderWithItemsAsync(OrderId, trader.Id);


           
            order.Status = OrderStatus.InProgress;

           
            foreach (var item in order.OrderItems)
            {
                item.OrderItemStatus = OrderItemStatus.InProgress;
             _unitOfWork.Repository<OrderItem>().Update(item);
            }

          
            _unitOfWork.Repository<Order>().Update(order);

            
            await _unitOfWork.Complet();

          
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع")]
        public async Task<IActionResult> GetInvoice(int orderid)
        {
            var trader=await _userManager.GetUserAsync(User);
            var order = await _orderRepository.GetOrderWithItemsAsync(orderid,trader.Id);

            return View(order);
        }


        public async Task<IActionResult> DownloadPhotos(int Id, int OrderId)
        {
            var orderitem = await _unitOfWork.Repository<OrderItem>().GetByIdAsync(Id);
            var photos = ImageService.ConvertJsonToUrls(orderitem.ProductItem.Photos);

            // Create a temporary folder to store the images before zipping
            string folderName = $"OrderNum{OrderId + 1000}_{orderitem.ProductItem.Name}_Photos";
            string tempFolderPath = Path.Combine(Path.GetTempPath(), folderName);
            Directory.CreateDirectory(tempFolderPath);

            using (HttpClient client = new HttpClient())
            {
                // Download each photo and save it to the temp folder
                foreach (var photoUrl in photos)
                {
                    var photo = $"http://giftlyapp.runasp.net/Custome/Image/{photoUrl}";
                    var response = await client.GetAsync(photo);
                    if (response.IsSuccessStatusCode)
                    {
                        string fileName = Path.GetFileName(photoUrl);
                        string filePath = Path.Combine(tempFolderPath, fileName);
                        await using var fileStream = new FileStream(filePath, FileMode.Create);
                        await response.Content.CopyToAsync(fileStream);
                    }
                }
            }

            // Create a ZIP file from the temp folder with the same name as the folder
            string zipFilePath = Path.Combine(Path.GetTempPath(), $"{folderName}.zip");
            ZipFile.CreateFromDirectory(tempFolderPath, zipFilePath);

            // Clean up the temporary folder
            Directory.Delete(tempFolderPath, true);

            // Return the ZIP file as a downloadable file
            byte[] fileBytes = System.IO.File.ReadAllBytes(zipFilePath);
            return File(fileBytes, "application/zip", $"{folderName}.zip");
        }

    }
}

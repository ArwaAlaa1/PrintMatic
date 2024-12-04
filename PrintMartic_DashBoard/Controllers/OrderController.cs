using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintMartic_DashBoard.Extention;
using PrintMartic_DashBoard.ViewModels;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
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
        private readonly INotificationRepository _notification;

        public OrderController(IUnitOfWork unitOfWork ,IMapper  mapper,IOrderRepository orderRepository
            ,UserManager<AppUser> userManager,INotificationRepository notification)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _notification = notification;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes ="Cookies",Roles ="بائع,Admin")]
        public async Task<IActionResult> Index(string status = "All")
        {
            OrderStatus parsedStatus;
            var isStatusValid = Enum.TryParse(status, ignoreCase: true, out parsedStatus);
            OrderItemStatus parsedItemStatus;
            var StatusValid = Enum.TryParse(status, ignoreCase: true, out parsedItemStatus);

            var allOrders =new List<Order>();
            var user = await _userManager.GetUserAsync(User);
            if (User.IsInRole("بائع"))
            {
                 allOrders = status == "All"
              ? await _orderRepository.GetOrdersForSpecificCompanyAsync(user.Id).ToListAsync()
              : isStatusValid ? await (_orderRepository.GetOrdersForSpecificCompanyAsync(user.Id, parsedItemStatus))
                 .ToListAsync()
                  : new List<Order>();

            

            }
            else if(User.IsInRole("Admin"))
            {
                 allOrders = status == "All"
             ? await _orderRepository.GetOrdersForAdminAsync().ToListAsync()
             : isStatusValid ? await (_orderRepository.GetOrdersForAdminAsync())
                 .Where(o => o.Status == parsedStatus).ToListAsync()
                 : new List<Order>();

                
            }
            var orders = _mapper.Map<List<Order>, List<OrderViewModelForCompany>>(allOrders);
            foreach (var order in orders)
            {
                var customer = await _userManager.FindByEmailAsync(order.CustomerEmail);
                order.CustomerUserName = customer?.UserName;
            }
            ViewBag.CurrentStatus = parsedStatus.GetEnumMemberValue(); 
            return View(orders);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع,Admin")]
        public async Task<IActionResult> OrderDetails(int OrderId)
        {
            var order = new Order();
            var user=await _userManager.GetUserAsync(User);
            if (User.IsInRole("بائع"))
            {
               order = await _orderRepository.GetOrderWithItemsForSpecificCompanyAsync(OrderId, user.Id);
               
            }else if (User.IsInRole("Admin"))
            {
                 order = await _orderRepository.GetInvoiceForAdminAsync(OrderId);
            }
                    
            var ordermapped = _mapper.Map<Order, OrderViewModelForCompany>(order);
            return View(ordermapped);
        
        }

        
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع,Admin")]
        public async Task<IActionResult> ConfirmOrder(int OrderId)
        {
            

            var order = new Order();
            var trader = await _userManager.GetUserAsync(User);
            if (User.IsInRole("بائع"))
            {
                 order = await _orderRepository.GetOrderWithItemsForSpecificCompanyAsync(OrderId, trader.Id);

                order.Status = OrderStatus.InProgress;

                foreach (var item in order.OrderItems)
                {
                    item.OrderItemStatus = OrderItemStatus.InProgress;
                    _unitOfWork.Repository<OrderItem>().Update(item);
                }


            }else if (User.IsInRole("Admin"))
            {
                order = await _orderRepository.GetOrderForAdminAsync(OrderId);

                order.Status = OrderStatus.InProgress;

                foreach (var item in order.OrderItems)
                {
                    item.OrderItemStatus = OrderItemStatus.InProgress;
                    _unitOfWork.Repository<OrderItem>().Update(item);
                }


            }
            _unitOfWork.Repository<Order>().Update(order);

            
            await _unitOfWork.Complet();

            return RedirectToAction("Index", "Order", new { status = "InProgress" });
          
        }

        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        public async Task<IActionResult> CancelOrder(int OrderId)
        {
            
             var order = await _orderRepository.CancelOrderForUserAsync(OrderId);

            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.Complet();


            return RedirectToAction("Index", "Order", new { status = "Cancelled" });
        }

        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        public async Task<IActionResult> CancelItem(int ItemId)
        {
            var orderItem = await _orderRepository.CanceltOrderItemForAdminAsync(ItemId);

           
            _unitOfWork.Repository<OrderItem>().Update(orderItem);

            await _unitOfWork.Complet();


            return RedirectToAction("OrderDetails", new {OrderId=orderItem.OrderId});
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin,بائع")]
        public async Task<IActionResult> GetInvoice(int orderid)
        {
            var order = new Order();
            var trader=await _userManager.GetUserAsync(User);
            if (User.IsInRole("بائع"))
            {
                order = await _orderRepository.GetInvoiceForTraderAsync(orderid, trader.Id);
               
            }
            if (User.IsInRole("Admin"))
            {
                order = await _orderRepository.GetInvoiceForAdminAsync(orderid);
            }
            return View(order);
        }


        [Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع,Admin")]
        public async Task<IActionResult> ReadyOrder(int orderid)
        {


            var order = new Order();
            var trader = await _userManager.GetUserAsync(User);
            if (User.IsInRole("بائع"))
            {
                order = await _orderRepository.GetOrderWithItemsForSpecificCompanyAsync(orderid, trader.Id);

                order.StatusReady = OrderReady.Ready;

                foreach (var item in order.OrderItems)
                {
                    item.OrderItemStatus = OrderItemStatus.Ready;
                    _unitOfWork.Repository<OrderItem>().Update(item);
                }


            }
            else if (User.IsInRole("Admin"))
            {
                order = await _orderRepository.GetOrderForAdminAsync(orderid);

                order.StatusReady = OrderReady.Ready;

                foreach (var item in order.OrderItems)
                {
                    item.OrderItemStatus = OrderItemStatus.Ready;
                    _unitOfWork.Repository<OrderItem>().Update(item);
                }


            }
            _unitOfWork.Repository<Order>().Update(order);


            await _unitOfWork.Complet();
            return NoContent();

        }


        public async Task<IActionResult> DownloadPhotos(int Id, int OrderId)
        {
            var orderitem = await _unitOfWork.Repository<OrderItem>().GetByIdAsync(Id);
            var photos = ImageService.ConvertJsonToUrls(orderitem.ProductItem.Photos);

            // Ensure folder name uniqueness using timestamp
            string folderName = $"OrderNum{OrderId + 1000}_{orderitem.ProductItem.Name}_Photos_{DateTime.Now:yyyyMMddHHmmss}";
            string tempFolderPath = Path.Combine(Path.GetTempPath(), folderName);
            Directory.CreateDirectory(tempFolderPath);

            using (HttpClient client = new HttpClient())
            {
                foreach (var photoUrl in photos)
                {
                    // Ensure the URL is complete
                    var photo = $"http://giftlyapp.runasp.net/Custome/Image/{photoUrl}";
                    try
                    {
                        var response = await client.GetAsync(photo);
                        if (response.IsSuccessStatusCode)
                        {
                            string fileName = Path.GetFileName(photoUrl);
                            string filePath = Path.Combine(tempFolderPath, fileName);
                            await using var fileStream = new FileStream(filePath, FileMode.Create);
                            await response.Content.CopyToAsync(fileStream);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error (optional)
                        Console.WriteLine($"Error downloading {photo}: {ex.Message}");
                    }
                }
            }

            // Create a ZIP file from the temp folder
            string zipFilePath = Path.Combine(Path.GetTempPath(), $"{folderName}.zip");
            try
            {
                ZipFile.CreateFromDirectory(tempFolderPath, zipFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating ZIP file: {ex.Message}");
                throw;
            }

            // Clean up the temporary folder
            try
            {
                Directory.Delete(tempFolderPath, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting temp folder: {ex.Message}");
            }

            // Return the ZIP file as a downloadable file
            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(zipFilePath);
                return File(fileBytes, "application/zip", $"{folderName}.zip");
            }
            finally
            {
                // Clean up the ZIP file after sending it
                if (System.IO.File.Exists(zipFilePath))
                {
                    System.IO.File.Delete(zipFilePath);
                }
            }
        }




        //public async Task<IActionResult> DownloadPhotos(int Id, int OrderId)
        //{
        //    var orderitem = await _unitOfWork.Repository<OrderItem>().GetByIdAsync(Id);
        //    var photos = ImageService.ConvertJsonToUrls(orderitem.ProductItem.Photos);

        //    // Create a temporary folder to store the images before zipping
        //    string folderName = $"OrderNum{OrderId + 1000}_{orderitem.ProductItem.Name}_Photos";
        //    string tempFolderPath = Path.Combine(Path.GetTempPath(), folderName);
        //    Directory.CreateDirectory(tempFolderPath);

        //    using (HttpClient client = new HttpClient())
        //    {
        //        // Download each photo and save it to the temp folder
        //        foreach (var photoUrl in photos)
        //        {
        //            var photo = $"http://giftlyapp.runasp.net/Custome/Image/{photoUrl}";
        //            var response = await client.GetAsync(photo);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                string fileName = Path.GetFileName(photoUrl);
        //                string filePath = Path.Combine(tempFolderPath, fileName);
        //                await using var fileStream = new FileStream(filePath, FileMode.Create);
        //                await response.Content.CopyToAsync(fileStream);
        //            }
        //        }
        //    }

        //    // Create a ZIP file from the temp folder with the same name as the folder
        //    string zipFilePath = Path.Combine(Path.GetTempPath(), $"{folderName}.zip");
        //    ZipFile.CreateFromDirectory(tempFolderPath, zipFilePath);

        //    // Clean up the temporary folder
        //    Directory.Delete(tempFolderPath, true);

        //    // Return the ZIP file as a downloadable file
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(zipFilePath);
        //    return File(fileBytes, "application/zip", $"{folderName}.zip");
        //}

    }
}

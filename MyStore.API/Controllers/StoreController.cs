using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyStore.DataAccess.DTOs;
using MyStore.DataAccess.Models;
using MyStoreBis;
using System.Security.Claims;
using MyStore.API;
//using MyStoreBis;
namespace MyStore.Controllers
{
    [ApiController]
    [Route("api/MyStore")]


    public class MyStoreController : ControllerBase
    {

        //// we added this for logger...
        //private readonly ILogger<MyStoreController> _logger;

        //public MyStoreController(ILogger<MyStoreController> logger)
        //{
        //    _logger = logger;
        //}


//// تعريف المتغيرات (هنا فقط)
    private readonly IAdTrackingService _adTrackingService;
    private readonly MyStoreContext _context;

    // مشيد واحد فقط يستقبل الخدمتين (هذا التصحيح)
    public MyStoreController(MyStoreContext context, IAdTrackingService adTrackingService)
    {
        _context = context;
        _adTrackingService = adTrackingService; 
    }

        [HttpGet("/api/MyStore/GetProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var business = new ProductBusiness(_context);
                var products = await business.GetAllProductsAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }



        [Authorize]
        [HttpPost("/api/MyStore/CreateOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
        {


            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }
            dto.UserId = userId;


            if (dto == null || dto.Items == null || !dto.Items.Any())
            {
                return BadRequest(new { message = "السلة فاضية أو البيانات ناقصة!" });
            }

            try
            {
                // استدعاء البزنس بشكل Async باستخدام await
                var orderBusiness = new MyStoreBis.AddOrderBusiness(_context);

                // استدعاء الدالة
                int newOrderId = await orderBusiness.InsertOrderAsync(dto);

var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (!string.IsNullOrEmpty(email))
                {
                    _ = _adTrackingService.TrackPurchaseAsync(email, dto);
                }


                return Ok(new
                {
                    message = "تم تسجيل طلبك بنجاح يا بطل (Async)!",
                    orderId = newOrderId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "صار خطأ في السيرفر: " + ex.Message });
            }
        }


        [Authorize(Roles = "Customer")]
        [HttpGet("/api/MyStore/GetMyOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyOrders()
        {





            Console.WriteLine("AUTH = " + User.Identity?.IsAuthenticated);

            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"{claim.Type} = {claim.Value}");
            }





            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "المستخدم غير معرّف أو جلسة الدخول منتهية." });
            }

            try
            {
                var business = new MyStoreBis.GetOrders(_context);
                var orders = await business.GetUserOrdersAsync(userId);

                if (orders == null)
                {
                    return NotFound(new { message = "No Orders Found!" });
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "صار خطأ في السيرفر: " + ex.Message });
            }








        }





        [Authorize]
        [HttpGet("/api/MyStore/GetCurrentUser")]
        public IActionResult GetCurrentUser()
        {
            //var userId = User.FindFirst("sub")?.Value;

            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            //var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                //userId = userId,
                username = username,
                //email = email,
                role = role
            });
        }


        [HttpPost("/api/MyStore/Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");

            return Ok(new
            {
                message = "تم تسجيل الخروج"
            });
        }







        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {

            var business = new MyStoreBis.GetAllOrders(_context);

            var orders = await business.GetAllOrdersAsync();

            return Ok(orders);
        }




        [HttpGet("/api/MyStore/Test")]
        public IActionResult Test()
        {
            return Ok(new
            {
                Message = "API Works"
            });
        }




    }

}

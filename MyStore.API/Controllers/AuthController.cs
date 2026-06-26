using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStore.DataAccess.DTOs;
using MyStore.DataAccess.Models;
using MyStoreBis;



namespace MyStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly MyStoreContext _context;


        public AuthController(MyStoreContext context)
        {
            _context = context;
        }

        [HttpPost("/api/MyStore/Users")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> insertUserAcync([FromBody] UserRegisterDto dto)
        {
           


            try
            {
                // استدعاء البزنس بشكل Async باستخدام await
                var UserBusiness = new MyStoreBis.AddUserBusiness(_context);

                // استدعاء الدالة
                int newUserId = await UserBusiness.InsertUserAsync(dto);


                if (newUserId == -1)
                {
                    return BadRequest(new
                    {
                        message = "هذا الإيميل مسجل مسبقاً!"
                    });
                }



                return Ok(new
                {
                    message = "تم انساء حسابك بنجاح (Async)!",
                   
                       newUserId =   newUserId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "صار خطأ في السيرفر: " + ex.Message });
            }
        }






        [HttpPost("/api/MyStore/Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            try
            {
                // استدعاء كلاس البزنس بأسلوب الـ new وبنمرر له الـ _context المشحون فوق جاهز
                var loginBis = new MyStoreBis.LoginBusiness(_context);

                // تشغيل دالة التحقق وصناعة التوكن
                var result = await loginBis.AuthenticateUserAsync(dto);


                // إذا نجح، نرجع كود 200 ومعه التوكن المشفر للريأكت
                Response.Cookies.Append("token", result.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.Now.AddDays(7)
                });

                return Ok(new
                {
                    message = "تم تسجيل الدخول بنجاح!",
                    username = result.Username
                });





            }
            catch (Exception ex)
            {
                // إذا البزنس رمى خطأ (إيميل أو باسورد غلط)، بيمسكه هنا ويرجع كود 401 (غير مصرح) مع رسالة الخطأ
                return Unauthorized(new { message = ex.Message });
            }
        }






    }
}

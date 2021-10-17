using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Thor.Models;
using Thor.Helpers;
using System.Linq;

namespace Thor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private string _password;
        private IConfiguration _configuration;

        public object JsonConvert { get; private set; }
        public object MimeMapping { get; private set; }

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
            _password = configuration.GetValue<string>("password");
        }

        [Route("/login")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return new ObjectResult(Constants.Messages.Success);
            }

            if (model.Password == _password)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, "user"));

                var identity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var props = new AuthenticationProperties();

                // This issues the authentication cookie
                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    props).Wait();

                return new ObjectResult(Constants.Messages.Success);
            }

            return StatusCode((int)HttpStatusCode.Unauthorized, Constants.Messages.UnauthorizedPassword);

        }

        [Route("/logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, Constants.Messages.UnauthorizedLogin);
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new ObjectResult(Constants.Messages.Success);
        }

        [HttpPost("/get_c")]
        public ActionResult GetC([FromBody] CModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, Constants.Messages.UnauthorizedLogin);
            }

            List<string> items = GenerateGetCItemListForResponse(model);
            return new OkObjectResult(new { items });
        }

        [HttpPost("/get_f")]
        public ActionResult GetF(ulong size)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, Constants.Messages.UnauthorizedLogin);
            }

            // Generates a random array of bytes
            Random rnd = new Random();
            byte[] randomBytesArray = new byte[size];
            rnd.NextBytes(randomBytesArray);

            // Insert you code here, you should read a file into a byte[] and replace the randomBytesArray
            return File(randomBytesArray, System.Net.Mime.MediaTypeNames.Application.Octet);
        }

        private List<string> GenerateGetCItemListForResponse(CModel model)
        {
            // For generating a random string (not a byte array...)
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random rnd = new();

            // Notice that sizeof(char) == 2
            uint totalBytes = model.Size;
            if (model.Type != CType.Type2) // Then we got size in bits, not bytes
            {
                totalBytes = model.Size / 8;
            }
            uint stringLength = totalBytes / sizeof(char);

            // Notice that 1 <= model.NumberOfItems always (default is 1), hence even if size == 0,
            // it will create an empty string and attach it to the list of items
            List<string> items = new();
            for (int i = 0; i < model.NumberOfItems; ++i)
            {
                items.Add(new string(Enumerable.Repeat(chars, (int)stringLength)
                    .Select(s => s[rnd.Next(s.Length)]).ToArray()));
            }

            return items;
        }
    }
}

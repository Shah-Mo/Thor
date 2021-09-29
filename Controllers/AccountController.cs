using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Thor.Models;

namespace Thor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private const int MAX_SIZE = 8192;
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
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return new ObjectResult("Success");
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

                return new ObjectResult("Success");
            }

            return StatusCode((int)System.Net.HttpStatusCode.Unauthorized, "Bad password");

        }

        [Route("/logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return StatusCode((int)System.Net.HttpStatusCode.Unauthorized, "Must login first");
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new ObjectResult("Success");
        }

        [HttpGet("/get_c")]
        public async Task<IActionResult> GetC()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return StatusCode((int)System.Net.HttpStatusCode.Unauthorized, "Must login first");
            }

            // Creating the mirror string to return
            var request = HttpContext.Request;

            if (!request.Body.CanSeek)
            {
                request.EnableBuffering();
            }

            request.Body.Position = 0;
            var reader = new StreamReader(request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync().ConfigureAwait(false);
            request.Body.Position = 0;
            
            // This return handles the response you actually need, it return a 200 with a string
            // Insert your code here (before the return) to edit the body variable
            return new OkObjectResult(new { message = body });
        }

        [HttpPost("/get_f")]
        public ActionResult GetF(ulong size)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return StatusCode((int)System.Net.HttpStatusCode.Unauthorized, "Must login first");
            }

            // Generates a random array of bytes
            Random rnd = new Random();
            byte[] randomBytesArray = new byte[size];
            rnd.NextBytes(randomBytesArray);

            // Insert you code here, you should read a file into a byte[] and replace the randomBytesArray
            return File(randomBytesArray, System.Net.Mime.MediaTypeNames.Application.Octet);
        }
    }
}

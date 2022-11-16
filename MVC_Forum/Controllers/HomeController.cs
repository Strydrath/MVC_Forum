using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using MVC_Forum.Models;
using System.Diagnostics;
using System.Globalization;

namespace MVC_Forum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public const string SessionKeyName = "_Name";
        public const string SessionKeyAdmin = "_IsAdmin";
        private static string language = "";

        private IStringLocalizer<HomeController> _localizer;
        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            setCulture();
        }

        private static void setCulture()
        {
            if (language == "")
            {
                language = "en-US";
            }
            CultureInfo ci = CultureInfo.GetCultureInfo(language);

            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        public IActionResult ChangeLanguage()
        {
            if (language == "pl-PL")
            {
                language = "en-US";
            }
            else
            {
                language = "pl-PL";
            }
            CultureInfo ci = CultureInfo.GetCultureInfo(language);


            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            return View("Index");
        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                return View("Login");
            }
            return View();
        }
        [Route("Login")]
        [HttpGet]
        public IActionResult LoginForm()
        {
            return View("Login");
        }
        [Route("Login/{username}")]
        [HttpGet]
        public IActionResult LoginTest(string username)
        {
            try
            {
                if (username != null)
                {
                    var found = UserController.Users.Single(user => user.Name == username);
                    if (found != null)
                    {
                        HttpContext.Session.SetString("_Name", username);
                        HttpContext.Session.SetString("_IsAdmin", found.IsAdmin.ToString());
                        return Redirect("Friends/List");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = "Invalid Account";
                return View("Login");

            }

            ViewBag.error = "Invalid Account";
            return View("Login");
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login(string username)
        {
            try
            {
                if (username != null)
                {
                    var found = UserController.Users.Single(user => user.Name == username);
                    if (found != null)
                    {
                        HttpContext.Session.SetString("_Name", username);
                        HttpContext.Session.SetString("_IsAdmin", found.IsAdmin.ToString());
                        return Redirect("Friends/List");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = "Invalid Account";
                return View("Login");

            }

            ViewBag.error = "Invalid Account";
            return View("Login");
        }

        [Route("Logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("_Name");
            HttpContext.Session.Remove("_IsAdmin");
            return RedirectToAction("Index");
        }

        [Route("Home/HandleError/{code:int}")]
        public IActionResult HandleError(int code)
        {
            ViewData["ErrorMessage"] = $"Error occurred. The ErrorCode is: {code}";
            return View("~/Views/Shared/HandleError.cshtml");
        }


        // GET: Home/Init
        [HttpGet]
        public ActionResult Init()
        {
            try
            {
                UserController.Users.Add(new User("Adam Małysz", false, new DateTime(2022, 11, 10)
                , new List<User> { UserController.Users[0], UserController.Users[1] }));
                UserController.Users.Add(new User("Test2", false, new DateTime(2022, 11, 4)

                    , new List<User> { UserController.Users[0], UserController.Users[1], UserController.Users[2] }));
                UserController.Users.Add(new User("User123", false, new DateTime(2022, 11, 6)));
                UserController.Users.Add(new User("ABCABC", false, new DateTime(2022, 11, 7)));
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
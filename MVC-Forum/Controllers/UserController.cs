using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_Forum.Models;

namespace MVC_Forum.Controllers
{
    public class UserController : Controller
    {
        public static List<User> Users = new List<User>()
        {
            new User("admin",true,new DateTime(2022,11,9)),
            new User("test",false,new DateTime(2022,11,9)),

        };
        
        // GET: UserController
        public ActionResult Index()
        {   
            return View(Users);
        }
        

        // GET: UserController/Create
        public ActionResult Create()
        {
            if (HttpContext.Session.GetString("_IsAdmin") == "false")
            {
                return RedirectToAction(nameof(Index));

            }
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {

            if (HttpContext.Session.GetString("_IsAdmin") == "false")
            {
                return RedirectToAction(nameof(Index));

            }
            try
            {
                if (Users.Find(user => user.Name == collection["Name"]) != null)
                {

                    Users.Add(new User(collection["Name"], (collection["IsAdmin"] != "false"), DateTime.Today));
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch
            {
                return View();
            }
        }



        // GET: UserController/Delete/5
        [Route("User/Delete/{name}")]
        public ActionResult Delete(string name)
        {
            if (HttpContext.Session.GetString("_IsAdmin") == "false")
            {
                return RedirectToAction(nameof(Index));

            }
            Users.RemoveAll(user => user.Name == name);
            return RedirectToAction(nameof(Index));
        }
        

       

    }
}

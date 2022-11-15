using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_Forum.Models;
using System.Text;
using NuGet.Protocol;

namespace MVC_Forum.Controllers
{
    public class FriendsController : Controller
    {
        // GET: FriendsController
        public ActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }

        // GET: FriendsController/Add/test
        [Route("Friends/Add/{name}")]
        public ActionResult Add(string name)    
        {
            User found = UserController.Users.Find(user => user.Name == name);
            if (found != null)
            {
                User? current = UserController.Users.Find(user => user.Name == HttpContext.Session.GetString("_Name"));
                current?.Friends.Add(found);
                return Ok(true);
            }
            return Ok(false);
        }
        [Route("Friends/Add")]
        [HttpPost]
        public ActionResult AddFriend(string name)
        {
            User found = UserController.Users.Find(user => user.Name == name);
            if (found != null)
            {
                User? current = UserController.Users.Find(user => user.Name == HttpContext.Session.GetString("_Name"));
                current?.Friends.Add(found);
                return Ok(true);
            }
            return Ok(false);
        }

        [Route("Friends/Delete/{name}")]
        public ActionResult Delete(string name)
        {
            User found = UserController.Users.Find(user => user.Name == name);
            if (found != null)
            {
                User? current = UserController.Users.Find(user => user.Name == HttpContext.Session.GetString("_Name"));
                current?.Friends.Remove(found);
                return Ok(true);
            }
            return Ok(false);
        }

        [Route("Friends/List")]
        public ActionResult List()
        {
            User? current = UserController.Users.Find(user => user.Name == HttpContext.Session.GetString("_Name"));
            var friends = current?.Friends;
            if (friends != null)
            {
                return View(friends);
            }
            return RedirectToAction("HomeController.Login");
        }

        public ActionResult Export()
        {
            User? current = UserController.Users.Find(user => user.Name == HttpContext.Session.GetString("_Name"));
            var friends = current?.Friends;
            var text = "";
            foreach (var friend in friends)
            {
                text += friend.Name + ", ";
            }
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return File(bytes, "text/plain", "friends.txt");
        }

        [HttpPost]
        public ActionResult Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var line = "";
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                line = reader.ReadLine();
            }

            var names = line.Split(", ");
            User? current = UserController.Users.Find(user => user.Name == HttpContext.Session.GetString("_Name"));
            current?.Friends.Clear();
            foreach (var name in names)
            {
                var found = UserController.Users.Find(user => user.Name == name);
                if (found != null)
                {
                    current?.Friends.Add(found);
                }
            }
            return RedirectToAction("List");
        }
    }

}

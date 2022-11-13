using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_Forum.Models;

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
                HomeController.CurrentUser.Friends.Add(found);
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
                HomeController.CurrentUser.Friends.Remove(found);
                return Ok(true);
            }
            return Ok(false);
        }

        public ActionResult List()
        {
            return View(HomeController.CurrentUser.Friends);
        }

    }
}

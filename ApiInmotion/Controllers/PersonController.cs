using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApiInmotion.Controllers
{
    public class PersonController : Controller
    {
        private InmotionEntities _inmotionContext = new InmotionEntities();
        // GET: Person
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllUsers()
        {
            //var users = _inmotionDBContext.User.ToList();

            return Json(_inmotionContext.Person.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveUser(Person user)
        {
            //user.Name = "Ulises";
            //user.Address = "Puru";
            //user.Rol = 1;
            //user.Gender = 1;
            _inmotionContext = new InmotionEntities();
            int idU = _inmotionContext.Person.Count();
            user.Id = idU + 1;
            _inmotionContext.Person.Add(user);
            int savedRecords = _inmotionContext.SaveChanges();
            var result = new { savedRecords = savedRecords };
            return Json(result);
        }
    }
}
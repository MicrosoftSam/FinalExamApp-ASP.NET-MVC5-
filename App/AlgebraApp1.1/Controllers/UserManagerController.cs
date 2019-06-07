using AlgebraApp1._1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AlgebraApp1._1.Controllers
{
    public class UserManagerController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        // GET: UserManager
        public ActionResult Index(string searchString)
        {
            if (isAdminUser())
            {
                var users = context.Users.ToList();
                if (!String.IsNullOrEmpty(searchString))
                {
                    users = context.Users.Where(u => u.UserName.Contains(searchString)).ToList();
                }

                return View(users);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        public ActionResult Edit(string id)
        {
            ApplicationUser user = GetUserById(id);
            ViewBag.Name = new SelectList(context.Roles.Where(r => !r.Name.Contains("Admin"))
                                            .ToList(), "Name", "Name");
            return View(user);
            
        }

        [HttpPost]
        public ActionResult Edit(ApplicationUser user)
        {
            ViewBag.Name = new SelectList(context.Roles.Where(r => !r.Name.Contains("Admin"))
                                               .ToList(), "Name", "Name");
            var selectedValue = Request.Form["UserRoles"].ToString();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser editedUser = GetUserById(user.Id);
            if (ModelState.IsValid)
            {
                editedUser.Id = user.Id;
                editedUser.Email = user.Email;
                editedUser.EmailConfirmed = user.EmailConfirmed;
                editedUser.LockoutEnabled = user.LockoutEnabled;
                editedUser.LockoutEndDateUtc = user.LockoutEndDateUtc;
                editedUser.PasswordHash = user.PasswordHash;
                editedUser.LockoutEnabled = user.LockoutEnabled;
                editedUser.LockoutEndDateUtc = user.LockoutEndDateUtc;
                editedUser.PhoneNumber = user.PhoneNumber;
                editedUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                editedUser.TwoFactorEnabled = user.TwoFactorEnabled;
                editedUser.AccessFailedCount = user.AccessFailedCount;
                editedUser.UserName = user.UserName; 
                userManager.AddToRole(user.Id, selectedValue);

                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit");
            
        }

        public ActionResult Delete(string id)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = GetUserById(id);
            if (!UserManager.IsInRole(user.Id,"Admin"))
            {
                context.Users.Remove(user);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
            
        }

        public Boolean isAdminUser()
        {
            var user = User.Identity;
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var s = UserManager.GetRoles(user.GetUserId());
            if (s[0].ToString() == "Admin")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private ApplicationUser GetUserById(string id)
        {
            return context.Users.Find(id);
        }

    }
}
using AlgebraApp1._1.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AlgebraApp1._1.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class SignUpController : Controller
    {
        public AlgebraAppDBEntities _db = new AlgebraAppDBEntities();

        public ActionResult Index(string searchString)
        {
            var courseSignUp = _db.CourseSignUp.Where(p => p.Status == false);


            if (!String.IsNullOrEmpty(searchString))
            {
                courseSignUp = courseSignUp.Where(p => p.Name.Contains(searchString)
                                            || p.LastName.Contains(searchString)
                                            || p.Address.Contains(searchString));


                return View("Index", courseSignUp);
            }

           return View("Index", courseSignUp);
        }

        public ActionResult ShowAll()
        {
            var predbiljezbe = _db.CourseSignUp.ToList();
            return View(predbiljezbe);
        }

        //GET
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Edit(int id)
        {
            ViewBag.Name = new SelectList(_db.Course.ToList(),"IdCourse","CourseName");
            var editCourse = _db.CourseSignUp.Find(id);
            var course = _db.Course.Find(editCourse.IdCourse);
            ViewBag.nameOfCourse = course.CourseName;
            return View("Edit", editCourse);
        }

        //POST
        [Authorize(Roles = "Admin, Employee")]
        [HttpPost]
        public ActionResult Edit(CourseSignUp edited)
        {
            var selectedCourse = Request.Form["ChoseSeminar"].ToString();
            ViewBag.Name = new SelectList(_db.Course.ToList(), "IdCourse", "CourseName");
            CourseSignUp savedCourseSignUp = GetCourseSignUpById(edited.IdCourseSignUp);
            if (ModelState.IsValid)
            {
                if (selectedCourse == "")
                {
                    ViewBag.ErrorMessage = "Potrebno je odabrati seminar";
                    return View();
                }
                savedCourseSignUp.IdCourseSignUp = edited.IdCourseSignUp;
                savedCourseSignUp.Date = DateTime.Now;
                savedCourseSignUp.IdCourse = int.Parse(selectedCourse);
                savedCourseSignUp.Name = edited.Name;
                savedCourseSignUp.LastName = edited.LastName;
                savedCourseSignUp.Address = edited.Address;
                savedCourseSignUp.Email = edited.Email;
                savedCourseSignUp.Telephone = edited.Telephone;
                savedCourseSignUp.Status = edited.Status;

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit");
        }

        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Delete(int id)
        {
            var courseSignUp = GetCourseSignUpById(id);

            _db.CourseSignUp.Remove(courseSignUp);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        private CourseSignUp GetCourseSignUpById(int id)
        {
            return _db.CourseSignUp.Where(p => p.IdCourseSignUp == id).SingleOrDefault();
        }
    }

}
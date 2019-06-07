using AlgebraApp1._1.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AlgebraApp1._1.Controllers
{
    public class HomeController : Controller
    {
        public AlgebraAppDBEntities _db = new AlgebraAppDBEntities();


        public ActionResult Index(string searchString)
        {
            var course = _db.Course.Where(s => s.Filled != true);

            if (!String.IsNullOrEmpty(searchString))
            {
                course = course.Where(s => s.CourseName.Contains(searchString)
                                            || s.Description.Contains(searchString));

                return View(course);
            }

            return View(course);
        }

        public ActionResult Edit()
        {
            return RedirectToAction("Edit", "Course");
        }

        [HttpGet]
        public ActionResult ChoseCourse(int id)
        {
            //TODO if int null provjera.
            Course course = _db.Course.Find(id);
            ViewBag.nameOfCourse = course.CourseName;
            return View();
        }
       

        [HttpPost]
        public ActionResult ChoseCourse(int id, CourseSignUp newCourseSignUp)
        {
            Course course = _db.Course.Find(id);
            newCourseSignUp.Date = DateTime.Now;
            newCourseSignUp.IdCourse = course.IdCourse;
            if (ModelState.IsValid)
            {
                try
                {
                    newCourseSignUp.IdCourseSignUp = _db.CourseSignUp.Max(s => s.IdCourseSignUp) + 1;
                }
                catch
                {
                    newCourseSignUp.IdCourseSignUp = 0;
                }
                _db.CourseSignUp.Add(newCourseSignUp);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
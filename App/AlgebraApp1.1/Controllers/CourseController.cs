using AlgebraApp1._1.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AlgebraApp1._1.Controllers
{
    public class CourseController : Controller
    {
        public AlgebraAppDBEntities _db = new AlgebraAppDBEntities();
        public Course course = new Course();

        public ActionResult Index(string searchString)
        {
            var courses = _db.Course.Where(s => s.Filled != true);

            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.CourseName.Contains(searchString)
                                            || s.Description.Contains(searchString));

                return View("Index", courses);
            }

            return View("Index", courses);
        }

        public ActionResult ShowAll()
        {
            var courses = _db.Course.ToList();
            return View(courses);
        }

        [Authorize(Roles ="Admin, Employee")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="Admin, Employee")]
        public ActionResult Create(Course newCourse)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    newCourse.IdCourse = _db.Course.Max(s => s.IdCourse) + 1;
                }
                catch
                {
                    newCourse.IdCourse = 0;
                }
                _db.Course.Add(newCourse);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize(Roles ="Admin, Employee")]
        public ActionResult Edit(int id)
        {
            var editCourse = _db.Course.Find(id);
            var course = _db.Course.Find(editCourse.IdCourse);
            ViewBag.nameOfCourse = course.CourseName;
            return View(editCourse);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Edit(Course edited)
        {
            Course savedCourse = GetCourseById(edited.IdCourse);
            if (ModelState.IsValid)
            {
                savedCourse.IdCourse = edited.IdCourse;
                savedCourse.CourseName = edited.CourseName;
                savedCourse.Description = edited.Description;
                savedCourse.Date = edited.Date;
                savedCourse.Filled = edited.Filled;

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit");
        }

        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Delete(int id)
        {
            var course = GetCourseById(id);
            var courseSignUp = _db.CourseSignUp.Where(p => p.IdCourse == id).FirstOrDefault();

            var coursesToRemove = _db.CourseSignUp.Where(p => p.IdCourse == id).ToList();
            foreach(var c in coursesToRemove)
            {
                _db.CourseSignUp.Remove(c);
            }
            _db.Course.Remove(course);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        private Course GetCourseById(int id)
        {
            return _db.Course.Where(s => s.IdCourse == id).SingleOrDefault();
        }
    }
}
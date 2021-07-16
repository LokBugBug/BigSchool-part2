using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext context;

        public CoursesController()
        {
            context = new ApplicationDbContext();
        }

        // GET: Courses



        public ActionResult Create()
        {
            BigSchoolContext context = new BigSchoolContext();

            Course objCourse = new Course();
            objCourse.ListCategory = context.Categories.ToList();
            return View(objCourse);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objCourse)
        {
            BigSchoolContext context = new BigSchoolContext();

            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                objCourse.ListCategory = context.Categories.ToList();
                return View("Create", objCourse);
            }
            //lấy ID user
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;
            //add vào csdl

            context.Courses.Add(objCourse);
            context.SaveChanges();

            //trở về home
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Attending()
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Attendance temp in listAttendances)
            {
                Course objCourse = temp.Course;
                objCourse.LetureName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }
        public ActionResult Mine()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            var courses = context.Courses.Where(c => c.LecturerId == currentUser.Id && c.DateTime > DateTime.Now).ToList();
            foreach (Course i in courses)
            {
                i.LetureName = currentUser.Name;
            }
            return View(courses);
        }
        [Authorize]
        //public ActionResult Edit(int id)
        //{

        //    BigSchoolContext1 context = new BigSchoolContext1();
        //    Course course = context.Courses.Single(c => c.Id == id);
        //    Course objCourse = new Course();
        //    objCourse.ListCategory = context.Categories.ToList();
        //    if (course == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(objCourse);
        //}
        //[Authorize]
        //[HttpPost]
        //public ActionResult Edit(Course course)
        //{
        //    BigSchoolContext1 context = new BigSchoolContext1();
        //    Course courseUpdate = context.Courses.Single(c => c.Id == course.Id);
        //    if (courseUpdate != null)
        //    {
        //        context.Courses.AddOrUpdate(courseUpdate);
        //        context.SaveChanges();

        //    }
        //    return RedirectToAction("Mine");
        //}
        public ActionResult Edit(int id)
        {
            BigSchoolContext context = new BigSchoolContext();
            Course c = context.Courses.SingleOrDefault(p => p.Id == id);
            c.ListCategory = context.Categories.ToList();
            return View(c);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Course c)
        {
            BigSchoolContext context = new BigSchoolContext();
            Course edit = context.Courses.SingleOrDefault(p => p.Id == c.Id);

            if (edit != null)
            {
                context.Courses.AddOrUpdate(c);
                context.SaveChanges();

            }
            return RedirectToAction("Mine");

        }

        public ActionResult Delete(int id)
        {
            BigSchoolContext context = new BigSchoolContext();
            Course course = context.Courses.SingleOrDefault(p => p.Id == id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCourse(Course c)
        {
            BigSchoolContext context = new BigSchoolContext();
            Course course = context.Courses.SingleOrDefault(p => p.Id == c.Id);
            if (course != null)
            {
                context.Courses.Remove(course);
                context.SaveChanges();

            }
            return RedirectToAction("Mine");
        }
        public ActionResult LectureIamGoing()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            var listFollwee = context.Followings.Where(p => p.FollowerId == currentUser.Id).ToList();
            var listAttendances = context.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (var course in listAttendances)
            {
                foreach (var item in listFollwee)
                {
                    if (item.FolloweeId == course.Course.LecturerId)
                    {
                        Course objCourse = course.Course;
                        objCourse.LetureName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                        courses.Add(objCourse);
                    }
                }
            }
            return View(courses);

        }
    }
}
        



   
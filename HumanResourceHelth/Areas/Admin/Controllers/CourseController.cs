using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using HumanResourceHelth.Web.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{
    public class CourseController : Controller
    {
        // GET: Admin/Course
        UnitOfWork _uow = new UnitOfWork();
        
        public ActionResult Index()
        {
            List<Course> courses = _uow.CourseRepo.GetAll();
            return View(courses);
        }

        public ActionResult Create()
        {
            CourseViewModel courseViewModel = new CourseViewModel();     
            return View("_Save", courseViewModel);
        }

        public ActionResult Edit(int courseId)
        {
            Course course = _uow.CourseRepo.FindById(courseId);
            CourseViewModel courseViewModel = new CourseViewModel()
            {
                Id = courseId,
                Name = course.Name,
                Price = course.Price,
                Description = course.Description,
                Password = course.Password,
                UserName=course.UserName,
                PublishDate=course.PublishDate            
            };
            return View("_Save", courseViewModel);
        }
        public ActionResult Save(Course course)
        {
            course.PublishDate = DateTime.Now;
            course.LastModifidDate = DateTime.Now;
            _uow.CourseRepo.Add(course);
            _uow.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        
        [HttpPost]
        public ActionResult Update(Course course)
        {
            Course myCourse = _uow.CourseRepo.FindById(course.Id);
            myCourse.Name =course.Name;
            myCourse.UserName =course.UserName;
            myCourse.Price =course.Price;
            myCourse.Password =course.Password;
            myCourse.Description =course.Description;
            myCourse.LastModifidDate = DateTime.Now;

            try
            {
                _uow.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Error Happend");
            }
        }

        public ActionResult Delete(int id)
        {
           
            try
            {
                Course course = _uow.CourseRepo.FindById(id);
                _uow.CourseRepo.Delete(course);
                _uow.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Error Happend");
            }
        }
    }
}
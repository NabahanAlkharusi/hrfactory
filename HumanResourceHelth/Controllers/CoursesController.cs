using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using HumanResourceHelth.Model.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Controllers
{
    //[SessionAdminAuthorize]
    public class CoursesController : Controller
    {
        UnitOfWork _unitOfWork = new UnitOfWork();
        //private UserManager<AppUser> userManager;
        //public CoursesController (UserManager<AppUser> usrMgr)
        //{
          
        //    userManager = usrMgr;
        //}


        public ActionResult Index()
        {
            var courses = _unitOfWork.coursesRepository.GetAll().OrderByDescending(x => x.CourseID);
            var trainers = _unitOfWork.TrainersRepo.GetAll();
            var deprts = _unitOfWork.DepartmentRepo.GetAll();

            var mycourses = from c in courses
                            join t in trainers on c.TrainerID equals t.TrainerID into table1
                            from t in table1.ToList()
                            join d in deprts on c.DepartmentID  equals d.DepartmentID into table2
                            from d in table2.ToList()
                            select new CoursesViewModel
                            {
                                Courses = c,
                                Trainers = t,
                                Departments = d
                            };

            return View(mycourses.ToList());

        }

        public ActionResult Create(int? ID)
        {
            List<Departments> DepartmentsList = GetDepartments();
            ViewBag.DepartmentsList = new SelectList(DepartmentsList, "DepartmentID", "Name");
            ViewBag.TrainersList = GetTrainersList();
            if (ID != null)
            {
                var courses = _unitOfWork.coursesRepository.FindById((int)ID);
                ViewBag.coursesAvatar = courses.Avatar;
           //     ViewBag.crsDt = courses.CourseDate.ToString("yyyy-MM-dd");
                return View(courses);
            }
            else
            {
                return View(new Courses());

            }

        }

        //[HttpPost]
        //public IActionResult Create(Courses course)
        //{
        //    course.CreatedBy = 1;
        //    if (course.CourseID != null || course.CourseID != 0)
        //    {
        //        course.ModifiedDate = DateTime.Today;
        //        _unitOfWork.Courses.Update(course);

        //    }
        //    else {
        //        course.CreatedDate = DateTime.Today;
        //        _unitOfWork.Courses.Add(course);
        //    }

        //    _unitOfWork.Complete();

        //    return RedirectToAction("Index", "Courses");

        //}

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file, Courses course)
        {
            var user_id = (User)HttpContext.Session["User"];
            var FileName = "";
            if (file != null && file.ContentLength > 0)
            {
                FileName = DateTime.Now.ToString("yyyyMMddhhmmssffftt") + "-" + file.FileName;
                course.Avatar = FileName;
            }
            else
            {
                var crs = _unitOfWork.coursesRepository.FindById(course.CourseID);
                course.Avatar = crs.Avatar;

            }

            if (course.CourseID != 0)
            {
                course.ModifiedBy = user_id.Name;
                course.ModifiedDate = DateTime.Today;
                _unitOfWork.coursesRepository.Update(course);
            }
            else
            {
                course.CreatedBy = user_id.Name;
                course.CreatedDate = DateTime.Today;
                _unitOfWork.coursesRepository.Add(course);
            }

            _unitOfWork.coursesRepository.SaveChanges();

            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(FileName);
                var path = Path.Combine(Server.MapPath("~/Images/coursesAvatar/"), fileName);
                file.SaveAs(path);
            }

          

            return RedirectToAction("Index", "Courses");
        }
        public ActionResult Delete(int ID)
        {
            var courses = _unitOfWork.coursesRepository.FindById((int)ID);
            return View(courses);
        }

        [HttpPost]
        public ActionResult Delete(Courses courses)
        {
          //  courses.ModifiedDate = DateTime.Today;
            _unitOfWork.coursesRepository.Remove(courses.CourseID);
            _unitOfWork.coursesRepository.SaveChanges();
            return RedirectToAction("Index", "Courses");

        }

        private SelectList GetTrainersList()
        {
            return new SelectList(_unitOfWork.TrainersRepo.GetAll().OrderByDescending(x => x.TrainerID).
                   Select(X => new
                   {
                       TrainerID = X.TrainerID,
                       TrainerName = X.TrainerName
                   }).ToList(), "TrainerID", "TrainerName");
        }


        private static List<Departments> GetDepartments()
        {
            string constr = @"Data Source=.;Initial Catalog=HODb;User ID=sa;Integrated Security=True";
            var constrObj = new UnitOfWork();
            constr = constrObj.GetConnectionString;
            List<Departments> Departments = new List<Departments>();
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SELECT DepartmentID, Name FROM Departments order by DepartmentID desc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Departments.Add(new Departments
                            {
                                Name = sdr["Name"].ToString(),
                                DepartmentID = Convert.ToInt32(sdr["DepartmentID"])
                            });
                        }
                    }
                    con.Close();
                }
            }

            return Departments;
        }



    }
}

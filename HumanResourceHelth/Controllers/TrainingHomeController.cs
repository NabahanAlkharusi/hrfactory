using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using HumanResourceHelth.Model.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Controllers
{
    public class TrainingHomeController : Controller
    {

        UnitOfWork _unitOfWork = new UnitOfWork();
        private UserManager<AppUser> userManager;
        //public TrainingHomeController(UserManager<AppUser> usrMGr)
        //{


        //    userManager = usrMGr;
        //}


        //[Authorize(Roles = "User")]
        [SessionAuthorizeAttribute]
        public ActionResult Checkout(int CID)
        {
            //string UserId = "";
            //Session["UserId"] = Request.QueryString["UserId"].ToString();
            //    UserId = Session["UserId"].ToString();
            string UserId = "";
            if (Session["UserId"] != null)
                UserId = Session["UserId"].ToString();
            if (CID == 0)
            {
                return RedirectToAction("AllCourses", "TrainingHome");
            }
            var checkPayment = _unitOfWork.PaymentRepo.GetAll().Where(x => x.CourseID == CID &&
            x.UserID == UserId).ToList();
            if (checkPayment.Count > 0)
            {
                return RedirectToAction("CourseMaterial", "TrainingHome", new { CID = CID });
            }

            var courses = _unitOfWork.coursesRepository.FindById(CID);
            //var trainer = _unitOfWork.Trainers.GetById(courses.TrainerID);
            //ViewBag.TrainerName = trainer.TrainerName;
            //ViewBag.TrainerAvatar = trainer.TrainerAvatar;

            //IList<Courses> crs = null;
            //crs = (IList<Courses>)_unitOfWork.Courses.GetAll();

            return View(courses);
        }

        [HttpPost]
        public ActionResult CheckOut(Pyments pyments)
        {
            string UserId = "";
            if (Session["UserId"] != null)
                UserId = Session["UserId"].ToString();

            if (ModelState.IsValid)
            {
                pyments.PymentDate = DateTime.Now;

                pyments.UserID = UserId;
                pyments.PaymentMethod = "Card";
                _unitOfWork.PaymentRepo.Add(pyments);
                _unitOfWork.PaymentRepo.SaveChanges();
            }
            else
            {
                return RedirectToAction("CheckOut", "TrainingHome", new { CID = pyments.CourseID });
            }
            //  _unitOfWork.Pyments.Add(pyments);
            return RedirectToAction("CourseMaterial", "TrainingHome", new { CID = pyments.CourseID });

            //  return Redirect("TrainingHome/CourseMaterial")  ;
        }

        public ActionResult Index()
        {
            //var Attachments = _unitOfWork.Attachments.GetAll();
            var courses = _unitOfWork.coursesRepository.GetAll().OrderByDescending(x => x.CourseID);
            var trainers = _unitOfWork.TrainersRepo.GetAll();
            var joinList = courses.Join(trainers, crs => crs.TrainerID,
                trn => trn.TrainerID, (Courses, Trainers) =>
                new CoursesViewModel() { Courses = Courses, Trainers = Trainers }).ToList().Take(5);

            var hrSetting = _unitOfWork.HrSettingRepo.FindById(1);
            ViewBag.LearnOnlineText = hrSetting.LearnOnlineText;
            ViewBag.LearnOnlineVideo = hrSetting.LearnOnlineVideo;
            if (Session["UserId"] == null)
            {
                Session["eligableForFreeSU"] = true;
                Session["eligableForFreeMB"] = true;
                Session["eligableForFree"] = true;
            }
            else
            {
                int UserId = (int)Session["UserId"];
                User user = _unitOfWork.UserRepo.FindById(UserId);
                GetSubscription(user);
            }
            return View(joinList);

        }


        public ActionResult AllCourses()
        {
            var courses = _unitOfWork.coursesRepository.GetAll().OrderByDescending(x => x.CourseID);
            var trainers = _unitOfWork.TrainersRepo.GetAll();
            var joinList = courses.Join(trainers, crs => crs.TrainerID,
                trn => trn.TrainerID, (Courses, Trainers) =>
                new CoursesViewModel() { Courses = Courses, Trainers = Trainers }).ToList();
            return View(joinList);

        }

        public ActionResult CourseDetails(int CID)
        {
            string UserId = "";
            if (Session["UserId"] != null)
                UserId = Session["UserId"].ToString();

            if (CID == 0)
            {
                return RedirectToAction("Index", "TrainingHome");
            }
            var courses = _unitOfWork.coursesRepository.FindById(CID);
            var trainer = _unitOfWork.TrainersRepo.FindById(courses.TrainerID);
            ViewBag.TrainerName = trainer.TrainerName;
            ViewBag.TrainerAvatar = trainer.TrainerAvatar;
            ViewBag.TrainerBio = trainer.TrainerBio;
            ViewBag.reviewDetails = GetUserReview(CID);

            ViewBag.CoursePrice = courses.CoursePrice;
            var checkPayment = _unitOfWork.PaymentRepo.GetAll().Where(x => x.CourseID == CID
               && x.UserID == UserId).ToList();
            ViewBag.checkPayment = checkPayment.Count;

            IList<Courses> crs = null;
            crs = (IList<Courses>)_unitOfWork.coursesRepository.GetAll();

            TempData["Courses"] = crs.Take(5);

            return View(courses);

        }


        //  [Authorize(Roles = "User")]
        [SessionAuthorizeAttribute]
        public ActionResult CourseMaterial(int CID)
        {
            string UserId = Session["UserId"].ToString();

            if (CID == 0)
            {
                return RedirectToAction("Index", "TrainingHome");
            }
            if (CID != 0)
            {
                var crid = _unitOfWork.coursesRepository.FindById(CID);
                var checkPayment = _unitOfWork.PaymentRepo.GetAll().Where(x => x.CourseID == CID
                && x.UserID == UserId).ToList();

                if (checkPayment.Count == 0 && crid.CoursePrice != 0 && crid.isfree == false)
                {
                    if (crid != null)
                    {
                        return RedirectToAction("AllCourses", "TrainingHome");
                    }
                    else
                    {
                        return RedirectToAction("CheckOut", "TrainingHome", new { CID = CID });
                    }
                }
            }

            var Attachments = _unitOfWork.attachmentsRepo.GetAll().Where(x => x.CourseID == CID).OrderByDescending(x => x.CourseID);
            var courses = _unitOfWork.coursesRepository.FindById(CID);
            var trainer = _unitOfWork.TrainersRepo.FindById(courses.TrainerID);
            ViewBag.TrainerName = trainer.TrainerName;
            ViewBag.CourseID = courses.CourseID;
            ViewBag.IntroVideo = courses.IntroVideo;
            ViewBag.CourseName = courses.CourseName;
            ViewBag.CreatedDate = courses.CreatedDate;
            ViewBag.Requirements = courses.Requirements;
            ViewBag.Viewpercentage = ViewPercentage(CID);
            ViewBag.reviewDetails = GetUserReview(CID);

            //IList<Courses> crs = null;
            //crs = (IList<Courses>)_unitOfWork.Courses.GetAll();

            //TempData["Courses"] = crs.Take(5);

            return View(Attachments);

        }



        public ActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        public ActionResult GetTrainers()
        {
            var trainers = _unitOfWork.TrainersRepo.GetAll();

            return View(trainers);
        }

        public JsonResult GetAllTrainers()
        {
            IList<Trainers> trainers = null;
            // trainers = _unitOfWork.TrainersRepo.GetAll();

            trainers = (IList<Trainers>)_unitOfWork.TrainersRepo.GetAll();
            if (trainers.Count == 0)
            {
                return Json("NotFound");
            }
            return Json(trainers.Take(5), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllCourses()
        {
            IList<Courses> course = null;
            course = (IList<Courses>)_unitOfWork.coursesRepository.GetAll().OrderByDescending(x => x.CourseID);
            if (course.Count == 0)
            {
                return Json("NotFound");
            }
            return Json(course.Take(5));
        }

        public JsonResult GetCourseDetails(int CourseID)
        {
            IList<Courses> course = null;
            course = (IList<Courses>)_unitOfWork.coursesRepository.FindById(CourseID);
            if (course.Count == 0)
            {
                return Json("NotFound");
            }
            return Json(course);
        }

        [HttpGet]
        public ActionResult GetBlobDownload(string link)
        {
            var fileName = link;
            link = Path.Combine(Server.MapPath("~/CoursesAttachments/"), link);

            //  link = @"~/CoursesAttachments/" + link;
            var net = new System.Net.WebClient();
            var data = net.DownloadData(link);
            var content = new System.IO.MemoryStream(data);
            var contentType = "APPLICATION/octet-stream";
            return File(content, contentType, fileName);
        }

        [HttpPost]
        public double UpdateViewDetails(int AttachID)
        {
            string UserId = Session["UserId"].ToString();

            var course = _unitOfWork.attachmentsRepo.FindById(AttachID);
            var courseViewed = _unitOfWork.UserCourceViewRepo.GetAll().Where(x => x.CourseID == course.CourseID
             && x.UserID == UserId).ToList();
            if (courseViewed.Where(x => x.AttachID == AttachID).Count() == 0)
            {
                var viewInsert = new UserCourseView();
                viewInsert.AttachID = AttachID;
                viewInsert.UserID = UserId;


                viewInsert.CourseID = course.CourseID;
                _unitOfWork.UserCourceViewRepo.Add(viewInsert);
                _unitOfWork.UserCourceViewRepo.SaveChanges();
                var viewPercentage = ViewPercentage(course.CourseID);
                return viewPercentage;
            }
            else
            {

                var viewPercentage = ViewPercentage(course.CourseID);
                return viewPercentage;
            }
        }

        private double ViewPercentage(int courseID)
        {
            string UserId = "";
            if (Session["UserId"] != null)
                UserId = Session["UserId"].ToString();


            var courseViewed = _unitOfWork.UserCourceViewRepo.GetAll().Where(x => x.CourseID == courseID
            && x.UserID == UserId).ToList();
            var totalVedios = _unitOfWork.attachmentsRepo.
                    GetAll().Where(x => x.CourseID == courseID &&
                   (x.AttachType == "youtube" || x.AttachType == "vimeo")).
                    Select(y => y.AttachID).Count();
            var viewCount = courseViewed.Where
                   (x => x.CourseID == courseID
                   && x.UserID == UserId).Count();
            var viewPercentage = 0;
            if (totalVedios != 0)
            {
                viewPercentage = (viewCount * 100) / totalVedios;
            }
            return viewPercentage;
        }
        [HttpPost]
        public bool UpdateReview(string reviewText, int starCount, int courseID)
        {

            var user_id = (User)HttpContext.Session["User"];

            int uid = (int)Session["UserId"];

            User user = _unitOfWork.UserRepo.Search(x => x.Id == uid).SingleOrDefault();

            try
            {
                var userRv = new UserReview();
                var userID = user_id.Id;
                userRv.ReviewText = reviewText;
                userRv.CourseID = courseID;
                userRv.FeedBackCount = starCount;
                userRv.ReviewUser = user_id.Name;
                userRv.ReviewDate = DateTime.Now;
                _unitOfWork.UserviewRepo.Add(userRv);
                _unitOfWork.UserviewRepo.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        private IList<UserReview> GetUserReview(int couseID)
        {
            var review = _unitOfWork.UserviewRepo.GetAll().Where(x => x.CourseID == couseID).ToList();
            return review;
        }
        public void GetSubscription(User user)
        {
            bool havingMBSub = false;
            bool havingSUSub = false;
            Session["eligableForFreeSU"] = false;
            Session["eligableForFreeMB"] = false;
            List<UserPlan> subscriptionPlan = _unitOfWork.UserPlanRepo.Search(x => x.UserId == user.Id && x.IsActive).ToList();
            List<UserPlan> subscriptionPlan1 = _unitOfWork.UserPlanRepo.Search(x => x.UserId == user.Id && x.IsActive).ToList();
            if (subscriptionPlan1.Count == 0)
            {
                Session["eligableForFree"] = true;
                Session["eligableForFreeSU"] = true;
                Session["eligableForFreeMB"] = true;
            }
            else
            {
                foreach (UserPlan Plansub in subscriptionPlan1)
                {
                    if (Plansub.IsFreeActive)
                    {
                        DateTime dateToday = DateTime.Now;
                        if ((Plansub.EndDate.Date - dateToday.Date).Days <= 0)
                        {
                            Plansub.IsActive = false;
                            Plansub.IsFreeActive = false;
                            _unitOfWork.SaveChanges();
                        }
                    }
                    if (Plansub.PlanId == (int)SubscriptionPlan.Startup)
                        havingSUSub = true;
                    if (Plansub.PlanId == (int)SubscriptionPlan.ManualBuilder)
                        havingMBSub = true;
                }
                if (!havingMBSub)
                    Session["eligableForFreeMB"] = true;
                if (!havingSUSub)
                    Session["eligableForFreeSU"] = true;
                if (!havingMBSub || !havingSUSub)
                    Session["eligableForFree"] = true;
                else
                    Session["eligableForFree"] = false;
            }
            Session["userPlans"] = subscriptionPlan;

        }

    }
}

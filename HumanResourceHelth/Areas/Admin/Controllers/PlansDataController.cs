using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Web.Areas.Admin.Data;
using HumanResourceHelth.Model;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{

    public class PlansDataController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();

        public ActionResult Index()
        {
            List<Plan> plans = _uow.PlanRepo.GetAll();

            PlanViewModel planViewModel = new PlanViewModel();
            return View(plans);
        }

        public ActionResult EditPlan(string plan)
        {
            CreateFolders(plan);

            DirectoryInfo[] myDirectoriesEnglish = new DirectoryInfo(Server.MapPath($"~/Areas/plansData/{plan}/CategoriesEnglish")).GetDirectories();
            DirectoryInfo[] myDirectoriesArabic = new DirectoryInfo(Server.MapPath($"~/Areas/plansData/{plan}/CategoriesArabic")).GetDirectories();

            PlanViewModel planView = new PlanViewModel()
            {
                DirectoriesEnglish = myDirectoriesEnglish,
                DirectoriesArabic = myDirectoriesArabic,
                plan = plan
            };
            return View(planView);
        }

        private void CreateFolders(string plan)
        {
            if (!Directory.Exists(Server.MapPath($"~/Areas/plansData/{plan}")))
                Directory.CreateDirectory(Server.MapPath($"~/Areas/plansData/{plan}"));

            if (!Directory.Exists(Server.MapPath($"~/Areas/plansData/{plan}/CategoriesArabic")))
                Directory.CreateDirectory(Server.MapPath($"~/Areas/plansData/{plan}/CategoriesArabic"));

            if (!Directory.Exists(Server.MapPath($"~/Areas/plansData/{plan}/CategoriesEnglish")))
                Directory.CreateDirectory(Server.MapPath($"~/Areas/plansData/{plan}/CategoriesEnglish"));
        }

        [HttpPost]
        public ActionResult Upload(string plan)
        {

            if (Request.Files.Count > 0)
            {
                try
                {


                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {


                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/Areas/plansData"), plan);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No file selected");
            }
        }

        [HttpPost]
        public ActionResult UploadFiles(string plan, string category, string lang)
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {

                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath($"~/Areas/plansData/{plan}/Categories{lang}/{category}"), fname);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No file selected");
            }
        }
        public ActionResult GridFiles(string plan, string Lang)
        {
            CreateFolders(plan);
            DirectoryInfo dir;
            if (Lang == "Arabic")
                dir = new DirectoryInfo(Server.MapPath($"~/Areas/plansData/{plan}/CategoriesArabic"));
            else
                dir = new DirectoryInfo(Server.MapPath($"~/Areas/plansData/{plan}/CategoriesEnglish"));

            ViewBag.Plan = plan;
            DirectoryInfo[] myDirectories = dir.GetDirectories();
            GridFilesViewModel gridFilesViewModel = new GridFilesViewModel()
            {
                PlanDirectory = myDirectories,
                Language = Lang
            };
            return PartialView(gridFilesViewModel);
        }
        public ActionResult DeleteFile(string plan, string directory, string file, string lang)
        {
            var myFile = Server.MapPath($"~/Areas/plansData/{plan}/Categories{lang}/{directory}/{file}");
            if (System.IO.File.Exists(myFile))
            {
                System.IO.File.Delete(myFile);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Error Happend");
            }
        }

        public ActionResult Price(int planId)
        {
            return View(_uow.PlanRepo.FindById(planId));
        }

        public ActionResult SavePrice(int planId, int annualPrice, int monthlyPrice)
        {
            Model.Plan plan = _uow.PlanRepo.FindById(planId);
            plan.AnnualPrice = annualPrice;
            plan.ManthlyPrice = monthlyPrice;
            _uow.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult UpdateCategoryName(string plan, string currentName, string newName, string lang)
        {
            string oldDir = Server.MapPath($"~/Areas/plansData/{plan}/Categories{lang}/{currentName}");
            string newDir = Server.MapPath($"~/Areas/plansData/{plan}/Categories{lang}/{newName}");
            try
            {
                Directory.Move( oldDir , newDir);
            }
            catch(Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
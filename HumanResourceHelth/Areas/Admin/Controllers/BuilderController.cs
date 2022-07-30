using HumanResourceHelth.DataAccess;
using HumanResourceHelth.DataAccess.Repositories;
using HumanResourceHelth.Model;
using HumanResourceHelth.Web.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Areas.Admin.Controllers
{
    public class BuilderController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();
        int userId = 0;
        // GET: Section
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return Redirect(Url.Action("Index", "Login", new { Area = "Admin", r = Request.Url.ToString() }));
            SectionViewModel section = new SectionViewModel()
            {
                Sections = _uow.SectionRepo.GetAll().Where(a => a.UserId == userId).OrderBy(a => a.Ordering).ToList()
            };

            return View(section);
        }
        public ActionResult Delete(int id)
        {
            Section section = _uow.SectionRepo.FindById(id);
            if (section.ParenId == null)
            {
                List<Section> sections = _uow.SectionRepo.GetAll().Where(a => a.ParenId == id).ToList();
                foreach (Section item in sections)
                {
                    _uow.SectionRepo.Delete(item);
                }
            }
            _uow.SectionRepo.Delete(section);
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
        public ActionResult Activate(int sectionId, bool isActive)
        {
            Section section = _uow.SectionRepo.FindById(sectionId);
            section.IsActive = isActive;
            foreach (var item in section.Childs)
            {
                item.IsActive = isActive;
            }
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
        public ActionResult Create()
        {
            int userId = int.Parse(Session["UserId"].ToString());
            SectionViewModel sectionViewModel = new SectionViewModel()
            {
                Sections = _uow.SectionRepo.GetAll().Where(a => a.ParenId == null && a.UserId == userId).ToList()
            };


            return View(sectionViewModel);
        }
        public ActionResult Save(Section section)
        {
            int userId = int.Parse(Session["UserId"].ToString());
            //get max order number
            int? countryMaxOrder = _uow.SectionRepo.GetAll().Where(a => a.ParenId == null && a.UserId == userId).Max(a => a.Ordering);
            int? cityMaxOrder = _uow.SectionRepo.GetAll().Where(a => a.ParenId == section.ParenId && a.UserId == userId).Max(a => a.Ordering);
            //check if null
            if (countryMaxOrder == null)
                countryMaxOrder = 0;
            if (cityMaxOrder == null)
                cityMaxOrder = 0;

            if (section.ParenId == null)
            {
                section.Ordering = countryMaxOrder + 1;
                section.UserId = int.Parse(Session["UserId"].ToString());
            }
            else
            {
                section.Ordering = cityMaxOrder + 1;
                section.UserId = int.Parse(Session["UserId"].ToString());
            }
            //int userId;
            if (Session["UserId"] != null)
                userId = (int)Session["UserId"];
            else
                userId = _uow.UserRepo.Search(x => x.IsAdmin).Single().Id;

            section.UserId = userId;
            _uow.SectionRepo.Add(section);
            _uow.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);


        }
        public void Convert()
        {
            //userId = (int)Session["UserId"];
            //Response.Clear();
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Disposition", "attachment; filename=hr.pdf");
            //string xHtml = HtmlString(userId);
            //var userData= _uow.SectionRepo.CompanyName(userId);
            //string data = userData.Name;
            //string docName = userData.DocumentName;
            //SimpleParser simpleParser = new SimpleParser();
            //string logoFilePath = "";

            //if (System.IO.File.Exists(Server.MapPath($"~/CompanyLogo/{userId}.png")))
            //{
            //    logoFilePath = Server.MapPath($"~/CompanyLogo/{userId}.png");
            //}
            //else
            //{
            //    logoFilePath = Server.MapPath($"~/CompanyLogo/default.png");
            //}

            //string cssPath = Server.MapPath("~/assets/css/pdfStyle.css");
            //simpleParser.Parse(Response.OutputStream, xHtml, data,docName, userId, logoFilePath,cssPath);
            //Response.End();
            //Response.Close();
        }
        public ActionResult SaveSectionOrder(int newOrder, int sectionId)
        {
            int userId = int.Parse(Session["UserId"].ToString());
            Section mainSection = _uow.SectionRepo.FindById(sectionId);
            if (mainSection.Ordering > newOrder)
            {
                List<Section> sectionInBetween = _uow.SectionRepo.GetAll().Where(a => a.ParenId == null && a.UserId == userId).Where(a => a.Ordering >= newOrder && a.Ordering < mainSection.Ordering).ToList();
                foreach (Section item in sectionInBetween)
                {
                    item.Ordering = item.Ordering + 1;
                }
                mainSection.Ordering = newOrder;
                _uow.SectionRepo.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            if (mainSection.Ordering < newOrder)
            {
                List<Section> sectionInBetween = _uow.SectionRepo.GetAll().Where(a => a.ParenId == null && a.UserId == userId).Where(a => a.Ordering <= newOrder && a.Ordering > mainSection.Ordering).ToList();
                foreach (Section item in sectionInBetween)
                {
                    item.Ordering = item.Ordering - 1;
                }
                mainSection.Ordering = newOrder;
                _uow.SectionRepo.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);

            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        public ActionResult SaveSubSectionOrder(int newOrder, int sectionId)
        {
            int userId = int.Parse(Session["UserId"].ToString());
            Section subSection = _uow.SectionRepo.FindById(sectionId);
            if (subSection.Ordering > newOrder)
            {
                List<Section> sectionInBetween = _uow.SectionRepo.GetAll().Where(a => a.ParenId == subSection.ParenId && a.UserId == userId).Where(a => a.Ordering >= newOrder && a.Ordering < subSection.Ordering).ToList();
                foreach (Section item in sectionInBetween)
                {
                    item.Ordering = item.Ordering + 1;
                }
                subSection.Ordering = newOrder;
                _uow.SectionRepo.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            if (subSection.Ordering < newOrder)
            {
                List<Section> sectionInBetween = _uow.SectionRepo.GetAll().Where(a => a.ParenId == subSection.ParenId && a.UserId == userId).Where(a => a.Ordering <= newOrder && a.Ordering > subSection.Ordering).ToList();
                foreach (Section item in sectionInBetween)
                {
                    item.Ordering = item.Ordering - 1;
                }
                subSection.Ordering = newOrder;
                _uow.SectionRepo.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);

            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        public ActionResult Edit(int sectionId)
        {
            Section section = _uow.SectionRepo.FindById(sectionId);
            return PartialView("_Edit", section);
        }
        [ValidateInput(false)]
        public ActionResult SaveContent(int sectionId, string title, string description, string content)
        {
            Section section = _uow.SectionRepo.FindById(sectionId);
            section.Title = title;
            section.Description = description;
            section.Content = content;
            _uow.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [HttpPost]
        public ActionResult UploadLogo()
        {
            if (Session["UserId"] == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No Active User");
            userId = (int)Session["UserId"];
            HttpFileCollectionBase files = Request.Files;
            if (!Directory.Exists(Server.MapPath($"~/CompanyLogo/")))
                Directory.CreateDirectory(Server.MapPath($"~/CompanyLogo/"));
            files[0].SaveAs(Server.MapPath($"~/CompanyLogo/{userId}.png"));
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
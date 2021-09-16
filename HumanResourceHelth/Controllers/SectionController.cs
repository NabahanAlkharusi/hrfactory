﻿using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Web.Data;
using HumanResourceHelth.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;

using System;
using HumanResourceHelth.Web.Models;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;


namespace HumanResourceHelth.Web.Controllers
{
    public class SectionController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();
        public ActionResult Index()
        {
            Session["Backto"] = "Section";
            if (Session["UserId"] == null)
                return RedirectToAction("Index", "Login");
            int userId = int.Parse(Session["UserId"].ToString());
            Language language = (string)Session["lang"] == "en-US" ? Language.English : Language.Arabic;
            SectionViewModel section = new SectionViewModel()
            {
                Sections = _uow.SectionRepo.Search(x => x.UserId == userId && x.LanguageId == (int)language).OrderBy(a => a.Ordering).ToList()
            };
            var vedios = _uow.introVedioRepo.GetAll();
            int vediosCount = vedios.Count;
            ViewBag.vediosCount = vediosCount;
            if (vediosCount > 0)
            {
                IntroVedio vedio = vedios.FirstOrDefault();
                ViewBag.ArabicUploaded = vedio.UploadedArabic;
                ViewBag.EnglishUploaded = vedio.UploadedEnglish;
                ViewBag.ArabicEmbaded = vedio.EmbadedArabic;
                ViewBag.EnglishEmbaded = vedio.EmbadedEnglish;
                ViewBag.IsUploaded = vedio.Uploaded;
            }

            return View(section);
        }
        public ActionResult Delete(int id)
        {
            HumanResourceHelth.Model.Section section = _uow.SectionRepo.FindById(id);
            if (section.ParenId == null)
            {
                List<HumanResourceHelth.Model.Section> sections = _uow.SectionRepo.GetAll().Where(a => a.ParenId == id).ToList();
                foreach (HumanResourceHelth.Model.Section item in sections)
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
            HumanResourceHelth.Model.Section section = _uow.SectionRepo.FindById(sectionId);
            section.IsActive = isActive;
            //foreach(var item in section.Childs)
            //{
            //    item.IsActive = isActive;
            //}

            if (Session["UserId"] == null)
                return RedirectToAction("Index", "Login");
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
            if (Session["UserId"] == null)
                return RedirectToAction("Index", "Login");
            int userId = int.Parse(Session["UserId"].ToString());
            Language language = (string)Session["lang"] == "en-US" ? Language.English : Language.Arabic;
            SectionViewModel sectionViewModel = new SectionViewModel()
            {
                Sections = _uow.SectionRepo.Search(a => a.ParenId == null && a.UserId == userId && a.LanguageId == (int)language).ToList()
            };


            return View(sectionViewModel);
        }
        public ActionResult Save(HumanResourceHelth.Model.Section section)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Index", "Login");
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
                section.IsHaveLineBefore = true;
            }
            else
            {
                section.Ordering = cityMaxOrder + 1;
                section.UserId = int.Parse(Session["UserId"].ToString());
                section.IsHaveLineBefore = false;
            }

            userId = (int)Session["UserId"];

            Language language = (string)Session["lang"] == "en-US" ? Language.English : Language.Arabic;
            section.LanguageId = (int)language;
            section.UserId = userId;
            _uow.SectionRepo.Add(section);
            _uow.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);


        }
        //public void Convert()
        //{
        //    Language language = (string)Session["lang"] == "en-US" ? Language.English : Language.Arabic;
        //    string templateView = language == Language.English ? "~/Views/Section/Template.cshtml" : "~/Views/Section/TemplateAr.cshtml";
        //    int userId = (int)Session["UserId"];
        //    User user = _uow.UserRepo.FindById(userId);
        //    Response.Clear();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("Content-Disposition", "attachment; filename=hr.pdf");
        //    List<Section> sections = _uow.SectionRepo.Search(x => x.ParenId == null && x.UserId == userId && x.IsActive && x.LanguageId == (int)language).OrderBy(x => x.Ordering).ToList();
        //    if (language == Language.Arabic)
        //        RemoveFontFamily(sections);
        //    string logoFilePath = GetLogoPath(userId);
        //    BuilderViewModel builderViewModel = new BuilderViewModel()
        //    {
        //        Sections = sections,
        //        DocumentName = language == Language.English ? user.DocumentName : user.DocumentNameAr,
        //        CompanyName = language == Language.English ? user.Name : user.NameAr,
        //        LogoFile = logoFilePath
        //    };
        //    string html = RenderViewToString(ControllerContext, templateView, builderViewModel, false);



        //    string companyName = user.Name;
        //    string documentName = user.DocumentName;
        //    SimpleParserr simpleParser = new SimpleParserr();

        //    string cssPath = Server.MapPath("~/assets/css/pdfStyle.css");
        //    simpleParser.Parse(Response.OutputStream, html, companyName, documentName, userId, logoFilePath, cssPath);
        //    Response.End();
        //    Response.Close();



        //}


        public void ConvertAr()
        {
            Language language = (string)Session["lang"] == "en-US" ? Language.English : Language.Arabic;
            string templateView = language == Language.English ? "~/Views/Section/Template.cshtml" : "~/Views/Section/TemplateAr.cshtml";
            int userId = (int)Session["UserId"];
            User user = _uow.UserRepo.FindById(userId);

            List<HumanResourceHelth.Model.Section> sections = _uow.SectionRepo.Search(x => x.ParenId == null && x.UserId == userId && x.IsActive && x.LanguageId == (int)language).OrderBy(x => x.Ordering).ToList();
            string logoFilePath = GetLogoPath(userId);
            BuilderViewModel builderViewModel = new BuilderViewModel()
            {
                Sections = sections,
                DocumentName = language == Language.English ? user.DocumentName : user.DocumentNameAr,
                CompanyName = language == Language.English ? user.Name : user.NameAr,
                LogoFile = logoFilePath
            };

            string strHtml = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            System.Web.UI.HtmlControls.HtmlGenericControl dvHtml = new System.Web.UI.HtmlControls.HtmlGenericControl();
            dvHtml.InnerHtml = RenderViewToString(ControllerContext, templateView, builderViewModel, false);
            dvHtml.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            strHtml = sr.ReadToEnd();
            sr.Close();

            Response.ContentType = "application/x-download";
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", "hr.pdf"));

            string companyName = user.Name;
            string documentName = user.DocumentName;
            DataAccess.Repositories.SimpleParserr simpleParser = new DataAccess.Repositories.SimpleParserr();

            string cssPath = Server.MapPath("~/assets/css/pdfStyle.css");
            simpleParser.Parse(Response.OutputStream, strHtml, companyName, documentName, userId, logoFilePath, cssPath);
            Response.Flush();
            Response.End();



        }


        private string GetLogoPath(int userId)
        {
            bool isLogoFileExist = System.IO.File.Exists(Server.MapPath($"~/CompanyLogo/{userId}.png"));
            return Server.MapPath(string.Format("~/CompanyLogo/{0}.png", isLogoFileExist ? userId.ToString() : "default"));
        }
        static string RenderViewToString(ControllerContext context, string viewPath, object model = null, bool partial = false)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial)
                viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            else
                viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("View cannot be found.");

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData, sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }
            return result;
        }
        public ActionResult SaveSectionOrder(int newOrder, int sectionId)
        {
            int userId = int.Parse(Session["UserId"].ToString());
            HumanResourceHelth.Model.Section mySection = _uow.SectionRepo.FindById(sectionId);
            if (mySection.Ordering > newOrder)
            {
                List<HumanResourceHelth.Model.Section> sectionInBetween = _uow.SectionRepo.GetAll().Where(a => a.ParenId == null && a.UserId == userId).Where(a => a.Ordering >= newOrder && a.Ordering < mySection.Ordering).ToList();
                foreach (HumanResourceHelth.Model.Section item in sectionInBetween)
                {
                    item.Ordering = item.Ordering + 1;
                }
                mySection.Ordering = newOrder;
                _uow.SectionRepo.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            if (mySection.Ordering < newOrder)
            {
                List<HumanResourceHelth.Model.Section> sectionInBetween = _uow.SectionRepo.GetAll().Where(a => a.ParenId == null && a.UserId == userId).Where(a => a.Ordering <= newOrder && a.Ordering > mySection.Ordering).ToList();
                foreach (HumanResourceHelth.Model.Section item in sectionInBetween)
                {
                    item.Ordering = item.Ordering - 1;
                }
                mySection.Ordering = newOrder;
                _uow.SectionRepo.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);

            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        public ActionResult SaveSubSectionOrder(int newOrder, int sectionId)
        {
            int userId = int.Parse(Session["UserId"].ToString());
            HumanResourceHelth.Model.Section subSection = _uow.SectionRepo.FindById(sectionId);
            if (subSection.Ordering > newOrder)
            {
                List<HumanResourceHelth.Model.Section> sectionInBetween = _uow.SectionRepo.GetAll().Where(a => a.ParenId == subSection.ParenId && a.UserId == userId).Where(a => a.Ordering >= newOrder && a.Ordering < subSection.Ordering).ToList();
                foreach (HumanResourceHelth.Model.Section item in sectionInBetween)
                {
                    item.Ordering = item.Ordering + 1;
                }
                subSection.Ordering = newOrder;
                _uow.SectionRepo.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            if (subSection.Ordering < newOrder)
            {
                List<HumanResourceHelth.Model.Section> sectionInBetween = _uow.SectionRepo.GetAll().Where(a => a.ParenId == subSection.ParenId && a.UserId == userId).Where(a => a.Ordering <= newOrder && a.Ordering > subSection.Ordering).ToList();
                foreach (HumanResourceHelth.Model.Section item in sectionInBetween)
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
            HumanResourceHelth.Model.Section section = _uow.SectionRepo.FindById(sectionId);
            return PartialView("_Edit", section);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveContent(int sectionId, string title, string description, string content, bool isNewPage)
        {
            HumanResourceHelth.Model.Section section = _uow.SectionRepo.FindById(sectionId);
            section.Title = title;
            section.Description = description;
            section.Content = content;
            section.IsHaveLineBefore = isNewPage;
            _uow.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [HttpPost]
        public ActionResult UploadLogo()
        {
            if (Session["UserId"] == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No Active User");
            int userId = (int)Session["UserId"];
            HttpFileCollectionBase files = Request.Files;
            if (!Directory.Exists(Server.MapPath($"~/CompanyLogo/")))
                Directory.CreateDirectory(Server.MapPath($"~/CompanyLogo/"));
            files[0].SaveAs(Server.MapPath($"~/CompanyLogo/{userId}.png"));
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        public ActionResult SaveIntroVedio(string vedioEmbadEn, string vedioEmbadAr, string isUploaded)
        {
            
            HttpFileCollectionBase files = Request.Files;
            var EnglishVedioFileName = "";
            var ArabicVedioFileName = "";
            if (files.Count > 0)
            {
                var test = isUploaded;
                 EnglishVedioFileName = Path.GetFileName(files[0].FileName);
                 ArabicVedioFileName = Path.GetFileName(files[1].FileName);
                if (!Directory.Exists(Server.MapPath($"~/IntroVedio/")))
                    Directory.CreateDirectory(Server.MapPath($"~/IntroVedio/"));
                files[0].SaveAs(Server.MapPath($"~/IntroVedio/{EnglishVedioFileName}"));
                files[1].SaveAs(Server.MapPath($"~/IntroVedio/{ArabicVedioFileName}"));
            }
            IntroVedio vedio = new IntroVedio();
            vedio.UploadedArabic = ArabicVedioFileName;
            vedio.UploadedEnglish = EnglishVedioFileName;
            vedio.EmbadedArabic = vedioEmbadAr;
            vedio.EmbadedEnglish = vedioEmbadEn;
            vedio.Uploaded = isUploaded == "1" ? true : false;
            _uow.introVedioRepo.Add(vedio);
            _uow.IndicatorRepo.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
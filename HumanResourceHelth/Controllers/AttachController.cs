using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using Microsoft.AspNet.Identity;
using System.Web.Routing;

namespace HumanResourceHelth.Web.Controllers
{
  //  [SessionAdminAuthorize]
public class AttachController : Controller
{
    UnitOfWork _unitOfWork = new UnitOfWork();
    //private UserManager<AppUser> userManager;

    //public AttachController(UserManager<AppUser> userMgr)
    //{


    //    userManager = userMgr;
    //}
    public ActionResult Index(int CID)
    {
        var Attachments = _unitOfWork.attachmentsRepo.GetAll().Where(x => x.CourseID == CID).OrderByDescending(x => x.AttachID);
        var courses = _unitOfWork.coursesRepository.FindById(CID);
        ViewBag.CourseName = courses.CourseName;
        return View(Attachments);
    }


    public ActionResult AddVideo(int CID, int? AttachID)
    {
        ViewBag.CID = CID;
        ViewBag.CoursesList = GetCoursesList();

        if (AttachID != null)
        {
            var attach = _unitOfWork.attachmentsRepo.FindById((int)AttachID);
            attach.CourseID = CID;
            return View(attach);
        }
        else
        {
            var Attachment = new Attachments();
            Attachment.CourseID = CID;
            return View(Attachment);
        }
    }

    [HttpPost]
    public async Task<ActionResult> AddVideo(Attachments Attachment)
    {

            var user_id = (User)HttpContext.Session["User"];

        if ( Attachment.AttachID != 0)
        {
                Attachment.ModifiedBy = user_id.Name;
            Attachment.ModifiedDate = DateTime.Today;
            _unitOfWork.attachmentsRepo.Update(Attachment);
        }
        else
        {
            Attachment.CreatedBy = user_id.Name; 
            Attachment.CreatedDate = DateTime.Today;
            _unitOfWork.attachmentsRepo.Add(Attachment);
        }
            _unitOfWork.SaveChanges();
        return RedirectToAction("index", new System.Web.Routing.RouteValueDictionary(
            new { action = "index", CID = Attachment.CourseID }));


    }


    public ActionResult AddFile(int CID, int? AttachID)
    {

        ViewBag.CID = CID;
        ViewBag.CoursesList = GetCoursesList();

        if (AttachID != null)
        {
            var attach = _unitOfWork.attachmentsRepo.FindById((int)AttachID);
            attach.CourseID = CID;
            return View(attach);
        }
        else
        {
            var Attachment = new Attachments();
            Attachment.CourseID = CID;
            return View(Attachment);
        }

    }

    [HttpPost]
    public async Task<ActionResult> AddFile(Attachments Attachment, HttpPostedFileBase file)
    {
        var user_id = (User)HttpContext.Session["User"];
            var FileName = "";
            if (Attachment.AttachType == "PDF" || Attachment.AttachType == "docx")
        {
            if (file != null || file.ContentLength != 0)
            {
                FileName = DateTime.Now.ToString("yyyyMMddhhmmssffftt") + "-" + file.FileName;
               Attachment.FilePath = FileName;
            }
        }

        if ( Attachment.AttachID != 0)
        {
            Attachment.ModifiedBy = user_id.Name;
            Attachment.ModifiedDate = DateTime.Today;
            _unitOfWork.attachmentsRepo.Update(Attachment);
        }
        else
        {
            Attachment.CreatedBy = user_id.Name;
            Attachment.CreatedDate = DateTime.Today;
            _unitOfWork.attachmentsRepo.Add(Attachment);
        }

        if (Attachment.AttachType == "PDF" || Attachment.AttachType == "docx")
        {

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(FileName);
                    var path = Path.Combine(Server.MapPath("~/CoursesAttachments/"), fileName);
                    file.SaveAs(path);
                }
            }

        _unitOfWork.SaveChanges();
        //return RedirectToAction(@"?CID=" + Attachment.CourseID, "Attach");
        return RedirectToAction("index", new RouteValueDictionary(
new { action = "index", CID = Attachment.CourseID }));

    }

    public ActionResult Delete(int AttachID)
    {
        var attachment = _unitOfWork.attachmentsRepo.FindById((int)AttachID);
        return View(attachment);
    }

    [HttpPost]
    public ActionResult Delete(Attachments attachment)
    {
        _unitOfWork.attachmentsRepo.Remove(attachment.AttachID);
        _unitOfWork.SaveChanges();
        return RedirectToAction("Index", new RouteValueDictionary(
new { controller = "Attach", action = "Index", CID = attachment.CourseID }));


    }

    private SelectList GetCoursesList()
    {
        return new SelectList(_unitOfWork.coursesRepository.GetAll().OrderByDescending(x => x.CourseID).
               Select(X => new
               {
                   CourseID = X.CourseID,
                   CourseName = X.CourseName
               }).ToList(), "CourseID", "CourseName");
    }

    [HttpPost]
    public async Task<ActionResult> UploadFile(HttpPostedFileBase file)
    {
        if (file == null || file.ContentLength == 0)
            return Content("file not selected");
        var path = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot",
                    file.FileName);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.InputStream.CopyToAsync(stream);
        }

        return RedirectToAction("Files");
    }



}
}

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
    public class VideoController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();
        // GET: Admin/Video
        public ActionResult Index(int CourseId)

        {
            ViewBag.CourseId = CourseId;
            List<Video> videos = _uow.VideoRepo.GetAll().Where(a => a.CourseId == CourseId).OrderBy(a=>a.Ordering).ToList();
            return View(videos);
        }

        public ActionResult Create(int courseId)
        {
            VideoViewModel videoViewModel = new VideoViewModel()
            {
                Courses = _uow.CourseRepo.GetAll(),
                CourseId=courseId
            };
        
            return View("_Save", videoViewModel);
        }


        public ActionResult Edit(int videoId)
        {
            Video video = _uow.VideoRepo.FindById(videoId);
            VideoViewModel videoViewModel = new VideoViewModel()
            {
                Id = videoId,
                Name = video.Name,
                IsForPriview=video.IsForPriview,
                Lenght=video.Lenght,
                Ordering=video.Ordering,
                Url=video.Url,
                CourseId=video.CourseId,
                Courses=_uow.CourseRepo.GetAll()   
            };
            return View("_Save", videoViewModel);
        }

        public ActionResult Save(Video video)
        {
            List<Video>videos =_uow.VideoRepo.GetAll().Where(a => a.CourseId == video.CourseId).ToList();
            if (videos.Count > 0)
            {
                int maxOrder = videos.Max(a => a.Ordering);
                video.Ordering = maxOrder + 1;
            }            
            else
            {
                video.Ordering = 1;
            }

            _uow.VideoRepo.Add(video);
            _uow.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult Update(Video video)
        {
            Video myVideo = _uow.VideoRepo.FindById(video.Id);
            myVideo.Name = video.Name;
            myVideo.Lenght = video.Lenght;
            myVideo.IsForPriview = video.IsForPriview;
            myVideo.CourseId = video.CourseId;
            myVideo.Ordering = video.Ordering;
            myVideo.Url = video.Url;
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
                Video video = _uow.VideoRepo.FindById(id);
                List<Video> videos = _uow.VideoRepo.GetAll().Where(a => a.CourseId == video.CourseId && a.Ordering>video.Ordering).ToList();
                _uow.VideoRepo.Delete(video);
                if (videos.Count>0)
                {
                    foreach (Video item in videos)
                    {
                        item.Ordering = item.Ordering - 1;
                    }
                } 
                _uow.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Error Happend");
            }
        }

        public ActionResult MoveUp(int id)
        {
            try
            {
                Video video = _uow.VideoRepo.FindById(id);
                Video upperVideo = _uow.VideoRepo.GetAll().Where(a=>a.CourseId==video.CourseId).FirstOrDefault(a => a.Ordering == (video.Ordering - 1));
                video.Ordering = video.Ordering-1;
                upperVideo.Ordering = upperVideo.Ordering + 1;
                _uow.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Error Happend");
            }
        }

        public ActionResult MoveDown(int id)
        {
            try
            {
                Video video = _uow.VideoRepo.FindById(id);
                Video lowerVideo = _uow.VideoRepo.GetAll().Where(a => a.CourseId == video.CourseId).FirstOrDefault(a => a.Ordering == (video.Ordering + 1));
                video.Ordering = video.Ordering + 1;
                lowerVideo.Ordering = lowerVideo.Ordering - 1;
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
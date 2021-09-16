using HumanResourceHelth.DataAccess;
using HumanResourceHelth.Model;
using HumanResourceHelth.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Controllers
{
    public class TrainersController : Controller
    {
        UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            var trainers = _unitOfWork.TrainersRepo.GetAll().OrderByDescending(x => x.TrainerID);
            var specialties = _unitOfWork.SpecialityRepo.GetAll();
            var joinList = trainers.Join(specialties, tr => tr.SpecialtyID,
                sp => sp.SpecialtyID, (Trainers, Specialties) =>
                new TrainersViewModel() { Trainers = Trainers, Specialties = Specialties }).ToList();

            return View(joinList);
        }

        public ActionResult Create(int? ID)
        {
            List<Specialties> SpecialtiesList = GetSpecialties();
            ViewBag.SpecialtiesList = new SelectList(SpecialtiesList, "SpecialtyID", "SpecialtyName");
            if (ID != null)
            {
                var trainers = _unitOfWork.TrainersRepo.FindById((int)ID);
                return View(trainers);
            }
            else
            {
                return View(new Trainers());

            }
        }

        [HttpPost]
        public ActionResult Create(Trainers trainer, HttpPostedFileBase file)
        {
            var user_id = Session["UserId"].ToString();
            var FileName = "";
            if (file != null)
            {
                FileName = DateTime.Now.ToString("yyyyMMddhhmmssffftt") + "-" + file.FileName;
            }

            if ( trainer.TrainerID != 0)
            {
                var trr = _unitOfWork.TrainersRepo.FindById(trainer.TrainerID);

                trainer.ModifiedBy = user_id;
                trainer.CreatedDate = trr.CreatedDate;
                trainer.ModifiedDate = DateTime.Today;
                if (file != null)
                {
                    trainer.TrainerAvatar = FileName;
                }
                else
                {
                    trainer.TrainerAvatar = trr.TrainerAvatar;
                    trr = trainer;
                    trainer = trr;
                }
                _unitOfWork.TrainersRepo.Update(trainer);
            }
            else
            {
                trainer.TrainerAvatar = FileName;
                trainer.CreatedBy = user_id;
                trainer.CreatedDate = DateTime.Today;
                _unitOfWork.TrainersRepo.Add(trainer);
            }


            _unitOfWork.TrainersRepo.SaveChanges();

            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(FileName);
                var path = Path.Combine(Server.MapPath("~/Images/TrainerAvatar/"), fileName);
                file.SaveAs(path);
            }


            return RedirectToAction("Index", "Trainers");


        }

        public ActionResult Delete(int ID)
        {
            var trainers = _unitOfWork.TrainersRepo.FindById((int)ID);
            return View(trainers);

        }

        [HttpPost]
        public ActionResult Delete(Trainers trainer)
        {
            trainer.ModifiedDate = DateTime.Today;
            _unitOfWork.TrainersRepo.Remove(trainer.TrainerID);
            _unitOfWork.TrainersRepo.SaveChanges();
            return RedirectToAction("Index", "Trainers");

        }


        private static List<Specialties> GetSpecialties()
        {
            var constrObj = new UnitOfWork();
            string constr = @"Data Source=NIASPC\EXPRESS;Initial Catalog=HrHealth;User ID=sa;Integrated Security=True";
            constr = constrObj.GetConnectionString;
            List<Specialties> specialties = new List<Specialties>();
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SELECT SpecialtyID, SpecialtyName FROM Specialties Order by SpecialtyID Desc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            specialties.Add(new Specialties
                            {
                                SpecialtyName = sdr["SpecialtyName"].ToString(),
                                SpecialtyID = Convert.ToInt32(sdr["SpecialtyID"])
                            });
                        }
                    }
                    con.Close();
                }
            }

            return specialties;
        }

    }
}

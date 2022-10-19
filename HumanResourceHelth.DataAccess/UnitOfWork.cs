using HumanResourceHelth.DataAccess.Repositories;
using System.Data.Entity;

namespace HumanResourceHelth.DataAccess
{
    public class UnitOfWork
    {
        public AttachmentsRepository attachmentsRepo;
        private readonly HumanResourceContext _entities;
        public SurveyTypeRepo SurveyTypeRepo { get; set; }
        public SurveyRepo SurveyRepo { get; set; }
        public UserRepo UserRepo { get; set; }
        public SurveyResultRepo SurveyResultRepo { get; set; }
        public AnswerRepo AnswerRepo { get; set; }
        public IndicatorRepo IndicatorRepo { get; set; }
        public QuestionRepo QuestionRepo { get; set; }
        public TechRequestRepo TechRequestRepo { get; set; }
        public DoctorRequestRepo DoctorRequestRepo { get; set; }
        public PluginRequestRepo PluginRequestRepo { get; set; }
        public CountryRepo CountryRepo { get; set; }
        public IndustryRepo IndustryRepo { get; set; }
        public SectionRepo SectionRepo { get; set; }

        public CourseRateRepo CourseRateRepo { get; set; }
        public CourseRepo CourseRepo { get; set; }
        public UserCourseRepo UserCourseRepo { get; set; }
        public UserWatchVideoRepo UserWatchVideoRepo { get; set; }
        public VideoRepo VideoRepo { get; set; }
        public UserPlanRepo UserPlanRepo { get; set; }
        public PlanRepo PlanRepo { get; set; }

        public ContentRepo ContentRepo { get; set; }

        public TrainersRepository TrainersRepo { get; set; }
        public DepartmentsRepository DepartmentRepo { get; set; }
        public CoursesRepository coursesRepository { get; set; }
        public SpecialtiesRepository SpecialityRepo { get; set; }
        public PymentsRepository PaymentRepo { get; set; }
        public UserCourseViewRepository UserCourceViewRepo { get; set; }
        public UserReviewRepository UserviewRepo { get; set; }
        public HrSettingRepository HrSettingRepo { get; set; }

        public CouponsRepo couponsRepo { get; set; }
        public IntroVedioRepo introVedioRepo { get; set; }
        public ExpertsProfileRepo ExpertsProfileRepo { get; set; }
        public TermsConditionsRepo TermsConditionsRepo { get; set; }
        public UpdatesRepo UpdatesRepo { get; set; }
        public SemiNotificationRepo SemiNotificationRepo { get; set; }
        public CategoryRepo CategoryRepo { get; set; }
        public DocFileRepo DocFileRepo { get; set; }
        public DefaultMBRepo DefaultMBRepo { get; set; }

        public FunctionsRepo FunctionsRepo { get; set; }
        public PartnersRepo PartnersRepo { get; set; }
        //public PlansToPartnerShipRepo PlansRepo { get; set; }
        public FunctionPracticeRepo FunctionPracticeRepo { get; set; }
        public PracticeQuestionsRepo PracticeQuestionsRepo { get; set; }
        public PartnershipRepo PartnershipRepo { get; set; }
        public PartnerShipPlansRepo PartnerShipPlansRepo { get; set; }

        #region Constructor
        public UnitOfWork() : this(null) { }
        public UnitOfWork(HumanResourceContext e, bool dropDb = false)
        {
            _entities = e ?? new HumanResourceContext();

            if (dropDb)
                DropDatabase(_entities);
            SurveyTypeRepo = new SurveyTypeRepo(_entities);
            SurveyRepo = new SurveyRepo(_entities);
            UserRepo = new UserRepo(_entities);
            SurveyResultRepo = new SurveyResultRepo(_entities);
            AnswerRepo = new AnswerRepo(_entities);
            IndicatorRepo = new IndicatorRepo(_entities);
            QuestionRepo = new QuestionRepo(_entities);
            PluginRequestRepo = new PluginRequestRepo(_entities);
            TechRequestRepo = new TechRequestRepo(_entities);
            DoctorRequestRepo = new DoctorRequestRepo(_entities);
            CountryRepo = new CountryRepo(_entities);
            IndustryRepo = new IndustryRepo(_entities);
            SectionRepo = new SectionRepo(_entities);
            CourseRateRepo = new CourseRateRepo(_entities);
            CourseRepo = new CourseRepo(_entities);
            UserCourseRepo = new UserCourseRepo(_entities);
            UserWatchVideoRepo = new UserWatchVideoRepo(_entities);
            VideoRepo = new VideoRepo(_entities);
            UserPlanRepo = new UserPlanRepo(_entities);
            PlanRepo = new PlanRepo(_entities);
            ContentRepo = new ContentRepo(_entities);
            attachmentsRepo = new AttachmentsRepository(_entities);
            TrainersRepo = new TrainersRepository(_entities);
            DepartmentRepo = new DepartmentsRepository(_entities);
            coursesRepository = new CoursesRepository(_entities);
            SpecialityRepo = new SpecialtiesRepository(_entities);
            PaymentRepo = new PymentsRepository(_entities);
            UserCourceViewRepo = new UserCourseViewRepository(_entities);
            UserviewRepo = new UserReviewRepository(_entities);
            HrSettingRepo = new HrSettingRepository(_entities);
            couponsRepo = new CouponsRepo(_entities);
            introVedioRepo = new IntroVedioRepo(_entities);
            ExpertsProfileRepo = new ExpertsProfileRepo(_entities);
            TermsConditionsRepo = new TermsConditionsRepo(_entities);
            UpdatesRepo = new UpdatesRepo(_entities);
            SemiNotificationRepo = new SemiNotificationRepo(_entities);
            CategoryRepo = new CategoryRepo(_entities);
            DocFileRepo = new DocFileRepo(_entities);
            DefaultMBRepo = new DefaultMBRepo(_entities);
            FunctionsRepo = new FunctionsRepo(_entities);
            PartnersRepo = new PartnersRepo(_entities);
            //PlansRepo = new PlansToPartnerShipRepo(_entities);
            FunctionPracticeRepo = new FunctionPracticeRepo(_entities);
            PracticeQuestionsRepo = new PracticeQuestionsRepo(_entities);
            PartnershipRepo = new PartnershipRepo(_entities);
            PartnerShipPlansRepo = new PartnerShipPlansRepo(_entities);
        }
        #endregion

        private static void DropDatabase(DbContext e)
        {
            e.Database.Delete();
        }

        public void SaveChanges()
        {
            _entities.SaveChanges();
        }

        public string GetConnectionString
        {
            get
            {
                return _entities.GetConstr();
            }
        }

    }
}

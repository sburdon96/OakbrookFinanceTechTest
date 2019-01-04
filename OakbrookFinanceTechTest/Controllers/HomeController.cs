namespace OakbrookFinanceTechTest.Controllers
{
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using OakbrookFinanceTechTest.Model;
    using OakbrookFinanceTechTest.Repository;
    using OakbrookFinanceTechTest.Repository.Interfaces;
    using OakbrookFinanceTechTest.Service;
    using OakbrookFinanceTechTest.Service.Interfaces;

    public class HomeController : AsyncController
    {
        private DetailsModel details;
        private IDetailsService detailsService;
        private IDetailsRepository detailsRepo;
        private IDecisionApiRepository decisionRepo;
        private ILogger logger;

        public HomeController()
        {
            this.details = new DetailsModel();
            this.logger = new Logger(ConfigurationManager.AppSettings["logFileDest"]);
            this.detailsRepo = new DetailsRepository(this.logger);
            this.decisionRepo = new DecisionApiRepository(this.logger);
            this.detailsService = new DetailsService(details, detailsRepo, decisionRepo, this.logger);
        }

        public ActionResult Index()
        {
            var model = this.details;
            return this.View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ProcessInput(DetailsModel model)
        {
            string partialView;
            if (model != null)
            {
                Task<DetailsModel> task = this.detailsService.ProcessInput(model);
                this.details = await task;

                if (model.Result.ToUpper() == "ACCEPTED")
                {
                    partialView = "AcceptedPartial";
                }
                else if (model.Result.ToUpper() == "DECLINED")
                {
                    partialView = "DeclinedPartial";
                }
                else
                {
                    partialView = "NoDecisionPartial";
                }
            }
            else
            {
                partialView = "ErrorPartial";
            }

            return this.PartialView(partialView, model);

        }
    }
}
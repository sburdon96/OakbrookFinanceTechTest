namespace OakbrookFinanceTechTest.Service
{
    using System;
    using System.Threading.Tasks;
    using OakbrookFinanceTechTest.Model;
    using OakbrookFinanceTechTest.Repository.Interfaces;
    using OakbrookFinanceTechTest.Service.Interfaces;

    public class DetailsService : IDetailsService
    {

        private IDetailsRepository detailsRepo;
        private IDecisionApiRepository decisionRepo;
        private ILogger logger;

        public DetailsService(DetailsModel details, IDetailsRepository detailsRepo, IDecisionApiRepository decisionRepo, ILogger logger)
        {
            this.detailsRepo = detailsRepo;
            this.decisionRepo = decisionRepo;
            this.logger = logger;
        }

        public async Task<DetailsModel> ProcessInput(DetailsModel details)
        {
            if (details != null)
            {
                Task<DetailsModel> restTask = this.PostDetails(details);

                if (details.Id == 0)
                {
                    var id = this.StoreDetails(details);
                    details = await restTask;
                    details.Id = id;
                }
                else
                {
                    details = await restTask;
                }

                if (details.Id != 0 && details.Result != null)
                {
                    this.StoreResult(details.Id, details.Result);
                }
            }
            else
            {
                details = new DetailsModel();
            }

            return details;
        }

        private Task<DetailsModel> PostDetails(DetailsModel details)
        {
           return this.decisionRepo.PostDetails(details);
        }

        private int StoreDetails(DetailsModel details)
        {
            return this.detailsRepo.StoreDetails(details);
        }

        private void StoreResult(int id, string result)
        {
            try
            {
                this.detailsRepo.StoreResult(id, result);
            }
            catch (Exception e)
            {
                this.logger.Log("DetailsService - StoreResult - " + e.Message);
            }
        }
    }
}

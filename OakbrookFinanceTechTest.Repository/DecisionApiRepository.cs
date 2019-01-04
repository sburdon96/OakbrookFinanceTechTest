namespace OakbrookFinanceTechTest.Repository
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using OakbrookFinanceTechTest.Model;
    using OakbrookFinanceTechTest.Repository.Interfaces;
    using RestSharp;
    public class DecisionApiRepository : IDecisionApiRepository
    {
        private ILogger logger;

        public DecisionApiRepository(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<DetailsModel> PostDetails(DetailsModel details)
        {
            //Request
            var client = new RestClient(ConfigurationManager.AppSettings["apiAddress"]);
            var request = new RestRequest("decision", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("firstName", details.FirstName);
            request.AddParameter("lastName", details.LastName);
            request.AddParameter("dateOfBirth", details.DateOfBirth);

            try
            {
                //Response
                var response = client.ExecuteTaskAsync(request);
                await response;
                var content = response.Result.Content;

                //Parse
                JObject jsonContent = JObject.Parse(content);
                JValue result = (JValue)jsonContent["decisionResult"];
                if (result == null)
                {
                    result = (JValue)jsonContent["Message"];
                }
                details.Result = result.ToString();
            }
            catch (Exception e)
            {
                this.logger.Log("DecisionApiRepository - PostDetails - " + e.Message);

                details.Result = "Error";
            }

            return details;
        }
    }
}

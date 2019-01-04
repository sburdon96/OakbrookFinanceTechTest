namespace OakbrookFinanceTechTest.Repository
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.Configuration;
    using Dapper;
    using OakbrookFinanceTechTest.Model;
    using OakbrookFinanceTechTest.Repository.Interfaces;

    public class DetailsRepository : IDetailsRepository
    {
        private ILogger logger;

        public DetailsRepository(ILogger logger)
        {
            this.logger = logger;
        }
        public int StoreDetails(DetailsModel details)
        {
            int userId;

            try
            {
                using (IDbConnection db = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog = UserDatabase; AttachDbFilename =|DataDirectory|\\UserDatabase.mdf;Integrated Security=True"))
                {
                    var sproc = "dbo.sproc_Insert_Details";
                    userId = db.Query<int>(sproc, details, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                this.logger.Log("DetailsRepository - StoreDetails - " + e.Message);

                userId = 0;

            }

            return userId;
        }

        public void StoreResult(int id, string result)
        {
            if (result.ToUpper() == "ACCEPTED" || result.ToUpper() == "DECLINED")
            {
                try
                {
                    using (IDbConnection db = new SqlConnection(WebConfigurationManager.ConnectionStrings["LocalDBConnectionString"].ConnectionString))
                    {
                        var sproc = "dbo.sproc_Update_Result";
                        db.Query<int>(sproc, param: new { id, result }, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception e)
                {
                    this.logger.Log("DetailsRepository - StoreResult - " + e.Message);
                }

            }
            else
            {
                // DO NOTHING
            }
        }
    }
}

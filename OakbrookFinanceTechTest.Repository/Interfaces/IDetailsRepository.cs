namespace OakbrookFinanceTechTest.Repository.Interfaces
{
    using OakbrookFinanceTechTest.Model;

    public interface IDetailsRepository
    {
        int StoreDetails(DetailsModel details);

        void StoreResult(int id, string result);
    }
}

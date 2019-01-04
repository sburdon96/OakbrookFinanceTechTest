namespace OakbrookFinanceTechTest.Repository.Interfaces
{
    using System.Threading.Tasks;
    using OakbrookFinanceTechTest.Model;

    public interface IDecisionApiRepository
    {
        Task<DetailsModel> PostDetails(DetailsModel details);
    }
}

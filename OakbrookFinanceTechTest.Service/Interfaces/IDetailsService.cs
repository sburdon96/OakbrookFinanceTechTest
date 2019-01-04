namespace OakbrookFinanceTechTest.Service.Interfaces
{
    using System.Threading.Tasks;
    using OakbrookFinanceTechTest.Model;

    public interface IDetailsService
    {
        Task<DetailsModel> ProcessInput(DetailsModel details);
    }
}

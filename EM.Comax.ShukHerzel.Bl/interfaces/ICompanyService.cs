using EM.Comax.ShukHerzel.Models.Models;

namespace EM.Comax.ShukHerzel.Bl.interfaces
{
    public interface ICompanyService
    {
        Task<List<Company>> GetAllCompanies();
    }
}

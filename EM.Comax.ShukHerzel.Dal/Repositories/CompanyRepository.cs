using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;

namespace EM.Comax.ShukHerzel.Dal.Repositories
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(ShukHerzelEntities context) : base(context)
        {
        }
    }
}

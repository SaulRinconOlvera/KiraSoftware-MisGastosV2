using KiraStudios.Domain.RepositoryBase;
using KiraStudios.Domain.UbicationModel.Ubication;
using KiraStudios.Domain.UbicationRepository;
using KiraStudios.Infrastructure.RepositoryBase;

namespace KiraStudios.Infraestructure.UbicationRepository.Ubication
{
    public class CountryRepository : RepositoryBase<int, Country>, ICountryRepository
    {
        public CountryRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}

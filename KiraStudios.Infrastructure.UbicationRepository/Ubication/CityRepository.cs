using KiraStudios.Domain.RepositoryBase;
using KiraStudios.Domain.UbicationModel.Ubication;
using KiraStudios.Domain.UbicationRepository;
using KiraStudios.Infrastructure.RepositoryBase;

namespace KiraStudios.Infraestructure.UbicationRepository.Ubication
{
    public class CityRepository : RepositoryBase<int, City>, ICityRepository
    {
        public CityRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}

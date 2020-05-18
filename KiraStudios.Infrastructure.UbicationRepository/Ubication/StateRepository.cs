using KiraStudios.Domain.RepositoryBase;
using KiraStudios.Domain.UbicationModel.Ubication;
using KiraStudios.Domain.UbicationRepository;
using KiraStudios.Infrastructure.RepositoryBase;

namespace KiraStudios.Infraestructure.UbicationRepository.Ubication
{
    public class StateRepository : RepositoryBase<int, State>, IStateRepository
    {
        public StateRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}

using KiraStudios.Application.ServiceBase;
using KiraStudios.Application.Services.UbicationContracts.Ubication;
using KiraStudios.Application.UbicationViewModel.Ubication;
using KiraStudios.Domain.UbicationModel.Ubication;
using KiraStudios.Domain.UbicationRepository;

namespace KiraStudios.Application.UbicationService.Ubication
{
    public class CountryAppService :
        ApplicationServiceBase<Country, CountryViewModel>, ICountryAppService
    {
        public CountryAppService(ICountryRepository repository) { _repository = repository; }
    }
}

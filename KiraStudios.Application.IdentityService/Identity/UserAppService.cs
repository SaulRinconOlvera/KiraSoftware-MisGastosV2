using KiraStudios.Application.MapperBase;
using KiraStudios.Application.ServiceBase;
using KiraStudios.Application.Services.IdentityContracts.Identity;
using KiraStudios.CrossCutting.Security.Token;
using KiraStudios.Domain.IdentityModel.Identity;
using KiraStudios.Domain.IdentityRepository;
using KiraSudios.Application.IdentityViewModel.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KiraStudios.Application.IdentityService.Identity
{
    public class UserAppService :
        ApplicationServiceBase<User, UserViewModel>, IUserAppService
    {
        private IRefreshTokenRepository _refreshTokenRepository;
        private IGenericMapper<RefreshToken, RefreshTokenViewModel> _refreshMapper;

        public UserAppService(
            IUserRepository repository,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _repository = repository;
            _refreshTokenRepository = refreshTokenRepository;
            _refreshMapper = new GenericMapper<RefreshToken,RefreshTokenViewModel>();
        }

        public async Task<UserViewModel> LoginAsync(UserViewModel userViewModel)
        {
            var entity = await ((IUserRepository)_repository)
                .LoginAsync(_mapper.GetEntity(userViewModel));

            if (entity is null) return null;
            return _mapper.GetViewModel(entity);
        }

        public async Task Logout() =>
            await ((IUserRepository)_repository).LogoutAsync();

        public async Task<UserViewModel> SocialNetwiorkLoginAsync(string userId, string platform)
        {
            var entity = await ((IUserRepository)_repository)
                .SocialNetwiorkLoginAsync(userId, platform);

            if (entity is null) return null;
            return _mapper.GetViewModel(entity);
        }

        public UserViewModel GetForModify(int viewModelId)
        {
            var entity = _repository.Get(viewModelId);
            return GetModelForModify(entity);
        }

        public async Task<UserViewModel> GetForModifyAsync(int viewModelId)
        {
            var entity = await _repository.GetAsync(viewModelId);
            return GetModelForModify(entity);
        }

        private UserViewModel GetModelForModify(User entity)
        {
            var model = _mapper.GetViewModel(entity);
            model.PasswordHash = entity.PasswordHash;
            model.SecurityStamp = entity.SecurityStamp;
            return model;
        }

        public async Task<RefreshTokenViewModel> GetRefreshToken(int viewModelId)
        {
            var result = await _refreshTokenRepository.GetAllMatchingAsync(rt => rt.UserId == viewModelId);
            if (result.Any()) return _refreshMapper.GetViewModel(result.FirstOrDefault());

            var newRefreshToken = GetNewRefreshToken(viewModelId);
            await _refreshTokenRepository.AddAsync(newRefreshToken);
            return _refreshMapper.GetViewModel(newRefreshToken);
        }

        public override UserViewModel AddWithResponse(UserViewModel viewModel)
        {
            viewModel = base.AddWithResponse(viewModel);
            _refreshTokenRepository.AddAsync(GetNewRefreshToken(viewModel.Id));

            return viewModel;
        }

        public override async Task<UserViewModel> AddWithResponseAsync(UserViewModel viewModel)
        {
            viewModel = await base.AddWithResponseAsync(viewModel);
            await _refreshTokenRepository.AddAsync(GetNewRefreshToken(viewModel.Id));
            return viewModel;
        }

        public override void Add(UserViewModel viewModel)
        {
            base.Add(viewModel);
            _refreshTokenRepository.AddAsync(GetNewRefreshToken(viewModel.Id));
        }

        public override async Task AddAsync(UserViewModel viewModel)
        {
            await base.AddAsync(viewModel);
            await _refreshTokenRepository.AddAsync(GetNewRefreshToken(viewModel.Id));
        }

        private RefreshToken GetNewRefreshToken(int viewModelId)
        {
            return new RefreshToken()
            {
                UserId = viewModelId,
                Token = TokenFactory.GenerateToken(64),
                CreatedBy = _serviceUser,
                CreationDate = DateTime.Now,
                LastModifiedBy = _serviceUser,
                LastModificationDate = DateTime.Now,
                Enabled = true
            };
        }
    }
}

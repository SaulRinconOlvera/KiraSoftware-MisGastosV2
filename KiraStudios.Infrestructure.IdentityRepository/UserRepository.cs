using KiraSolutions.Infrastructure.Configuration;
using KiraStudios.Domain.EntityBase.Contracts;
using KiraStudios.Domain.IdentityModel.Identity;
using KiraStudios.Domain.IdentityRepository;
using KiraStudios.Domain.RepositoryBase;
using KiraStudios.Infrastructure.RepositoryBase;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KiraStudios.Infraestructure.IdentityRepository
{
    public class UserRepository : RepositoryBase<int, User>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async override Task AddAsync(User entity, bool autoSave = true)
        {
            entity.ConcurrencyStamp = "";
            if (entity.Logins != null && entity.Logins.Any())
                entity.Logins.ToList().ForEach(l => SetAuthenticationValues(entity, l));

            if (entity.Claims != null && entity.Claims.Any())
                entity.Claims.ToList().ForEach(c => SetAuthenticationValues(entity, c));

            var result = await IdentityConfiguration.UserManager.CreateAsync(entity, entity.PasswordHash);
            if (!result.Succeeded) ErrorOnCreate(result.Errors);
        }

        private void SetAuthenticationValues(User entity, IBaseAuditable p)
        {
            p.CreatedBy = entity.CreatedBy;
            p.CreationDate = entity.CreationDate;
            p.LastModificationDate = entity.LastModificationDate;
            p.LastModifiedBy = entity.LastModifiedBy;
            p.DeletedBy = entity.DeletedBy;
            p.DeletionDate = entity.DeletionDate;
        }

        public override void Add(User entity, bool autoSave = true)
        {
            entity.ConcurrencyStamp = "";
            base.Add(entity, autoSave);
        }

        public override async Task ModifyAsync(User entity, bool autoSave = true)
        {
            entity.ConcurrencyStamp = "";

            // TODO: Encontrar como corregir este problema

            //await IdentityConfiguration.UserManager.UpdateSecurityStampAsync(entity);
            await IdentityConfiguration.UserManager.UpdateNormalizedEmailAsync(entity);
            await IdentityConfiguration.UserManager.UpdateNormalizedUserNameAsync(entity);
            //await IdentityConfiguration.UserManager.SetLockoutEnabledAsync(entity, true);

            await base.ModifyAsync(entity, autoSave);
        }

        public override void Modify(User entity, bool autoSave = true)
        {
            entity.ConcurrencyStamp = "";
            base.Modify(entity, autoSave);
        }

        public override async Task<User> GetAsync(int entityId)
        {
            var results = await GetAllMatchingAsync(u => u.Id == entityId, "Roles", "Claims", "Logins", "RefreshToken");
            var userResult = results.FirstOrDefault();
            userResult.RolesNames = await GetRolesNames(userResult);
            return userResult;
        }


        public async Task<User> LoginAsync(User entity)
        {
            var result = await IdentityConfiguration.SignManager
                .PasswordSignInAsync(entity.UserName, entity.PasswordHash, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded) return null;

            var results = await GetAllMatchingAsync(u => u.UserName == entity.UserName, "Roles", "Claims", "Logins", "RefreshToken");
            var userResult = results.FirstOrDefault();
            userResult.RolesNames = await GetRolesNames(userResult);
            return userResult;
        }

        public async Task LogoutAsync() =>
            await IdentityConfiguration.SignManager.SignOutAsync();

        public async Task<User> SocialNetwiorkLoginAsync(string userId, string platform)
        {
            var entity = await IdentityConfiguration.UserManager.FindByLoginAsync(platform, userId);
            if (entity is null) return null;

            await IdentityConfiguration.SignManager.SignInAsync(entity, false);

            var userResult = await IdentityConfiguration.UserManager.FindByNameAsync(entity.UserName);
            userResult.RolesNames = await GetRolesNames(userResult);
            return userResult;
        }

        private async Task<string> GetRolesNames(User userResult)
        {
            var result = await IdentityConfiguration.UserManager.GetRolesAsync(userResult);
            return string.Join('|', result);
        }

        private void ErrorOnCreate(IEnumerable<IdentityError> errors)
        {
            string cadena = string.Empty;
            errors.ToList().ForEach(e => cadena += $"Error:'{e.Description}'\n");
            throw new Exception(cadena);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Pleiades.Framework.Data;
using Pleiades.Framework.Helpers;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;


namespace Pleiades.Framework.Identity.Concrete
{
    /// <summary>
    /// Core Identity User Service
    /// </summary>
    public class IdentityUserService : IIdentityUserService
    {
        public const int MaxRootUsers = 1;
        public const int MaxAdminUsers = 5;

        public IGenericRepository<Model.IdentityUser> Repository { get; set; }

        public IdentityUserService(IIdentityRepository identityRepository)
        {
            this.Repository = identityRepository;
        }

        public IdentityUserService(IGenericRepository<Model.IdentityUser> repository)
        {
            this.Repository = repository;
        }

        public int GetUserCountByRole(IdentityUserRole role)
        {
            return this.Repository.GetAll().Where(x => x.UserRole == role).Count();
        }

        /// <summary>
        /// Generate a new Identity User - avoid creating quantity of Root & Admin Users beyond threshhold
        /// </summary>
        public Model.IdentityUser Create(CreateNewIdentityUserRequest newUserRequest)
        {
            if (newUserRequest.UserRole == IdentityUserRole.Admin)
            {
                var countAdmin = this.GetUserCountByRole(IdentityUserRole.Admin);
                if (countAdmin >= MaxAdminUsers)
                {
                    throw new Exception(String.Format("Maximum number of Admin Users is {0}", MaxAdminUsers));
                }
            }

            if (newUserRequest.UserRole == IdentityUserRole.Supreme)
            {
                var countRoot = this.GetUserCountByRole(IdentityUserRole.Supreme);
                if (countRoot >= MaxRootUsers)
                {
                    throw new Exception(String.Format("Maximum number of Root Users is 1", MaxRootUsers));
                }
            }

            // Create the Identity User EF Entity and Save
            var identityUserEntity = new Model.IdentityUser
            {
                AccountStatus = newUserRequest.AccountStatus,
                UserRole = newUserRequest.UserRole,
                AccountLevel = newUserRequest.AccountLevel,
                FirstName = newUserRequest.FirstName,
                LastName = newUserRequest.LastName,
                CreationDate = DateTime.Now,
                LastModified = DateTime.Now,
            };

            this.Repository.Add(identityUserEntity);
            this.Repository.SaveChanges();

            // Get the Identity User Aggregate Root we just added
            return RetrieveUserById(identityUserEntity.ID);
        }

        /// <summary>
        /// Retrieve Identity User by ID
        /// </summary>
        public Model.IdentityUser RetrieveUserById(int identityUserId)
        {
            var identityUserEntity = this.Repository.FindFirstOrDefault(x => x.ID == identityUserId);
            return identityUserEntity;
        }

        /// <summary>
        /// Retrieve total head count
        /// </summary>
        public int RetrieveTotalUsers()
        {
            return this.Repository.Count();
        }

        /// <summary>
        /// Update existing Identity User - will only modify Identity User entities, not Membership!
        /// </summary>
        public void Update(Model.IdentityUser changes)
        {
            var identityUserEntity = this.RetrieveUserById(changes.ID);
            
            identityUserEntity.UserRole = changes.UserRole;
            identityUserEntity.AccountStatus = changes.AccountStatus;
            identityUserEntity.AccountLevel = changes.AccountLevel;
            identityUserEntity.FirstName = changes.FirstName;
            identityUserEntity.LastName = changes.LastName;
            identityUserEntity.LastModified = DateTime.Now;

            this.Repository.SaveChanges();
        }

        /// <summary>
        /// Set the Last Modified footprint
        /// </summary>
        public void UpdateLastModified(Model.IdentityUser identityUser)
        {
            var identityUserEntity = this.RetrieveUserById(identityUser.ID);
            identityUserEntity.LastModified = DateTime.Now;
            this.Repository.SaveChanges();
        }

        /// <summary>
        /// Delete Identity User
        /// </summary>
        /// <param name="identityUser"></param>
        public void Delete(Model.IdentityUser identityUser)
        {
            var identityUserEntity = this.RetrieveUserById(identityUser.ID);
            if (identityUserEntity.UserRole == IdentityUserRole.Supreme)
            {
                throw new Exception("Illegal to delete Root User from application layer");
            }

            this.Repository.Delete(identityUserEntity);
            this.Repository.SaveChanges();
        }
    }
}
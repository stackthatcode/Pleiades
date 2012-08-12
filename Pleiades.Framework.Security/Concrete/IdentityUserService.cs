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
        public const int MaxSupremeUsers = 1;
        public const int MaxAdminUsers = 5;

        public IIdentityRepository Repository { get; set; }

        public IdentityUserService(IIdentityRepository identityRepository)
        {
            this.Repository = identityRepository;
        }

        public int GetUserCountByRole(UserRole role)
        {
            return this.Repository.GetUserCountByRole(role);
        }

        /// <summary>
        /// Generate a new Identity User - avoid creating quantity of Root & Admin Users beyond threshhold
        /// </summary>
        public Model.IdentityUser Create(CreateOrModifyIdentityUserRequest newUserRequest)
        {
            if (newUserRequest.UserRole == UserRole.Admin)
            {
                var countAdmin = this.GetUserCountByRole(UserRole.Admin);
                if (countAdmin >= MaxAdminUsers)
                {
                    throw new Exception(String.Format("Maximum number of Admin Users is {0}", MaxAdminUsers));
                }
            }

            if (newUserRequest.UserRole == UserRole.Supreme)
            {
                var countRoot = this.GetUserCountByRole(UserRole.Supreme);
                if (countRoot >= MaxSupremeUsers)
                {
                    throw new Exception(String.Format("Maximum number of Root Users is 1", MaxSupremeUsers));
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
            return identityUserEntity;
        }

        /// <summary>
        /// Retrieve Identity User by ID
        /// </summary>
        public Model.IdentityUser RetrieveUserById(int identityUserId)
        {
            var identityUserEntity = this.Repository.RetrieveUserById(identityUserId);
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
        public void Update(Model.CreateOrModifyIdentityUserRequest changes)
        {
            var identityUserEntity = this.Repository.RetrieveUserById(changes.ID);
            
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
        public void UpdateLastModified(int id)
        {
            var identityUserEntity = this.Repository.RetrieveUserById(id);
            identityUserEntity.LastModified = DateTime.Now;
            this.Repository.SaveChanges();
        }

        /// <summary>
        /// Delete Identity User
        /// </summary>
        /// <param name="identityUser"></param>
        public void Delete(int id)
        {
            var identityUserEntity = this.Repository.RetrieveUserById(id);
            if (identityUserEntity.UserRole == UserRole.Supreme)
            {
                throw new Exception("Illegal to delete Root User from application layer");
            }

            this.Repository.Delete(identityUserEntity);
            this.Repository.SaveChanges();
        }
    }
}
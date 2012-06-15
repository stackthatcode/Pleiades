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
    /// Core Domain User Service
    /// </summary>
    public class DomainUserService : IDomainUserService
    {
        public const int MaxRootUsers = 1;
        public const int MaxAdminUsers = 5;

        public IGenericRepository<Model.DomainUser> Repository { get; set; }

        public DomainUserService(IGenericRepository<Model.DomainUser> repository)
        {
            this.Repository = repository;
        }

        public int GetUserCountByRole(UserRole role)
        {
            return this.Repository.GetAll().Where(x => x.UserRole == role).Count();
        }

        /// <summary>
        /// Generate a new Domain User - avoid creating quantity of Root & Admin Users beyond threshhold
        /// </summary>
        public Model.DomainUser Create(CreateNewDomainUserRequest newUserRequest)
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
                if (countRoot >= MaxRootUsers)
                {
                    throw new Exception(String.Format("Maximum number of Root Users is 1", MaxRootUsers));
                }
            }

            // Create the Domain User EF Entity and Save
            var domainUserEntity = new Model.DomainUser
            {
                AccountStatus = newUserRequest.AccountStatus,
                UserRole = newUserRequest.UserRole,
                AccountLevel = newUserRequest.AccountLevel,
                FirstName = newUserRequest.FirstName,
                LastName = newUserRequest.LastName,
                CreationDate = DateTime.Now,
                LastModified = DateTime.Now,
            };

            this.Repository.Add(domainUserEntity);
            this.Repository.SaveChanges();

            // Get the Domain User Aggregate Root we just added
            return RetrieveUserById(domainUserEntity.ID);
        }

        /// <summary>
        /// Retrieve Domain User by ID
        /// </summary>
        public Model.DomainUser RetrieveUserById(int domainUserId)
        {
            var domainUserEntity = this.Repository.FindFirstOrDefault(x => x.ID == domainUserId);
            return domainUserEntity;
        }

        /// <summary>
        /// Retrieve total head count
        /// </summary>
        public int RetrieveTotalUsers()
        {
            return this.Repository.Count();
        }

        /// <summary>
        /// Update existing Domain User - will only modify Domain User entities, not Membership!
        /// </summary>
        public void Update(Model.DomainUser changes)
        {
            var domainUserEntity = this.RetrieveUserById(changes.ID);
            
            domainUserEntity.UserRole = changes.UserRole;
            domainUserEntity.AccountStatus = changes.AccountStatus;
            domainUserEntity.AccountLevel = changes.AccountLevel;
            domainUserEntity.FirstName = changes.FirstName;
            domainUserEntity.LastName = changes.LastName;
            domainUserEntity.LastModified = DateTime.Now;

            this.Repository.SaveChanges();
        }

        /// <summary>
        /// Set the Last Modified footprint
        /// </summary>
        public void UpdateLastModified(Model.DomainUser domainUser)
        {
            var domainUserEntity = this.RetrieveUserById(domainUser.ID);
            domainUserEntity.LastModified = DateTime.Now;
            this.Repository.SaveChanges();
        }

        /// <summary>
        /// Delete Domain User
        /// </summary>
        /// <param name="domainUser"></param>
        public void Delete(Model.DomainUser domainUser)
        {
            var domainUserEntity = this.RetrieveUserById(domainUser.ID);
            if (domainUserEntity.UserRole == UserRole.Supreme)
            {
                throw new Exception("Illegal to delete Root User from application layer");
            }

            this.Repository.Delete(domainUserEntity);
            this.Repository.SaveChanges();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Commerce.Domain.Interface;
using Pleiades.Data;
using Pleiades.Data.EF;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.Persist.Security
{
    public class AggregateUserRepository : EFGenericRepository<AggregateUser>, IAggregateUserRepository
    {
        public AggregateUserRepository(DbContext context) : base(context)
        {
        }

        // Exists to patch the Membership User's isolated Database Context issues
        public MembershipUser RetreiveMembershipUser(string membershipUsername)
        {
            return this.Context
                .Set<MembershipUser>()
                .FirstOrDefault(x => x.UserName == membershipUsername);
        }

        public AggregateUser RetrieveByMembershipUserName(string membershipUsername)
        {
            this.Context.Set<MembershipUser>();

            var output = this.TrackableData()
                .Include(x => x.IdentityProfile)
                .FirstOrDefault(x => x.Membership.UserName == membershipUsername);
            return output;   
        }

        public IEnumerable<AggregateUser> Retreive(List<UserRole> roles)
        {
            var enumRoles = roles.Select(x => x.ToString());
            return this.TrackableData().Where(x => enumRoles.Contains(x.IdentityProfile.UserRoleValue));
        }

        public IEnumerable<AggregateUser> Retreive(List<string> membershipUserNames, List<UserRole> roles)
        {
            var enumRoles = roles.Select(x => x.ToString());
            return this.ReadOnlyData().Where(x =>
                enumRoles.Contains(x.IdentityProfile.UserRoleValue) && 
                membershipUserNames.Contains(x.Membership.UserName));
        }

        public AggregateUser RetrieveById(int aggregateUserId)
        {
            return this.ReadOnlyData()
                .Include(x => x.IdentityProfile)
                .Include(x => x.Membership)
                .FirstOrDefault(x => x.ID == aggregateUserId);
        }

        protected AggregateUser RetrieveByIdForWriting(int aggregateUserId)
        {
            return this.TrackableData()
                .Include(x => x.IdentityProfile)
                .Include(x => x.Membership)
                .FirstOrDefault(x => x.ID == aggregateUserId);
        }


        public int GetUserCountByRole(UserRole role)
        {
            var roleName = role.ToString();
            return this.ReadOnlyData().Count(x => x.IdentityProfile.UserRoleValue == roleName);
        }

        /// <summary>
        /// Update existing Identity User - will only modify Identity User entities, not Membership!
        /// </summary>
        public void UpdateIdentity(int aggregateUserID, CreateOrModifyIdentityRequest changes)
        {
            var identity = this.RetrieveByIdForWriting(aggregateUserID).IdentityProfile;
            
            identity.UserRole = changes.UserRole;
            identity.AccountStatus = changes.AccountStatus;
            identity.AccountLevel = changes.AccountLevel;
            identity.FirstName = changes.FirstName;
            identity.LastName = changes.LastName;
            identity.LastModified = DateTime.Now;

            this.Context.SaveChanges();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Pleiades.Data;
using Pleiades.Data.EF;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.Persist.Security
{
    public class AggregateUserRepository : EFGenericRepository<AggregateUser>, IAggregateUserRepository
    {
        public AggregateUserRepository(PleiadesContext context) : base(context)
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

            var output = this.ReadOnlyData()
                .Include(x => x.IdentityProfile)
                .FirstOrDefault(x => x.Membership.UserName == membershipUsername);
            return output;   
        }

        public IEnumerable<AggregateUser> Retreive(List<UserRole> roles)
        {
            var enumRoles = roles.Select(x => x.ToString());
            return this.ReadOnlyData()
                .Include(x => x.IdentityProfile)
                .Include(x => x.Membership)
                .Where(x => enumRoles.Contains(x.IdentityProfile.UserRoleValue));
        }

        public IEnumerable<AggregateUser> Retreive(List<string> membershipUserNames, List<UserRole> roles)
        {
            var enumRoles = roles.Select(x => x.ToString());

            return this.ReadOnlyData()
                .Include(x => x.IdentityProfile)
                .Include(x => x.Membership)
                .Where(x =>
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

        public AggregateUser RetrieveByIdForWriting(int aggregateUserId)
        {
            return this.Data()
                .Include(x => x.IdentityProfile)
                .Include(x => x.Membership)
                .FirstOrDefault(x => x.ID == aggregateUserId);
        }

        public int GetUserCountByRole(UserRole role)
        {
            var roleName = role.ToString();
            return this.ReadOnlyData()
                .Count(x => x.IdentityProfile.UserRoleValue == roleName);
        }

        /// <summary>
        /// Update existing Identity User - will only modify Identity User entities, not Membership!
        /// </summary>
        public void UpdateIdentity(CreateOrModifyIdentityRequest changes)
        {
            var user = this.RetrieveByIdForWriting(changes.Id);
            var identity = user.IdentityProfile;
            var membership = user.Membership;

            if (changes.UserRole != null) 
                identity.UserRole = changes.UserRole.Value;
            if (changes.AccountStatus != null)
                identity.AccountStatus = changes.AccountStatus.Value;
            if (changes.AccountLevel != null)
                identity.AccountLevel = changes.AccountLevel.Value;
            if (changes.FirstName != null)
                identity.FirstName = changes.FirstName;
            if (changes.LastName != null)
                identity.LastName = changes.LastName;

            membership.LastModified = DateTime.Now;
        }

        public void Delete(int aggregateUserID)
        {
            var user = this.RetrieveByIdForWriting(aggregateUserID);
            this.Delete(user);
        }
    }
}
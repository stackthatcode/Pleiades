using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Pleiades.Web.Security.Model;
using PagedList;

namespace Pleiades.Web.Security.Interface
{
    /// <summary>
    /// Contains all CRUD operations for managing Domain Users
    /// </summary>
    public interface IDomainUserService
    {
        DomainUser Create(DomainUserCreateRequest newUserRequest, out MembershipCreateStatus createStatus);
        DomainUser RetrieveUserByDomainUserId(int domainUserId);
        DomainUser RetrieveUserByMembershipUserName(string username);
        DomainUser RetrieveUserByEmail(string emailaddr);
        IPagedList<DomainUserCondensed> RetreiveAll(int pageNumber, int pageSize, List<UserRole> role);
        IPagedList<DomainUserCondensed> RetreiveByLikeEmail(
            string emailAddressToMatch, int pageNumber, int pageSize, List<UserRole> role);
        int RetrieveTotalUsers();
        void Update(DomainUser user);
        void UpdateLastModified(DomainUser user);
        void Delete(DomainUser user);        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Framework.Data;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.Identity.Interface
{
    /// <summary>
    /// Contains all CRUD operations for managing Domain Users
    /// </summary>
    public interface IDomainUserService
    {        
        int GetUserCountByRole(UserRole role);
        DomainUser Create(CreateNewDomainUserRequest newUserRequest);
        DomainUser RetrieveUserById(int domainUserId);        
        int RetrieveTotalUsers();
        void Update(DomainUser user);
        void UpdateLastModified(DomainUser user);
        void Delete(DomainUser user);        
    }
}

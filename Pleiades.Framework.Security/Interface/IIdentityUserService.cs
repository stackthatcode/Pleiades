using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Framework.Data;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.Identity.Interface
{
    /// <summary>
    /// Contains all CRUD operations for managing Identity Users
    /// </summary>
    public interface IIdentityUserService
    {        
        int GetUserCountByRole(IdentityUserRole role);
        IdentityUser Create(CreateNewIdentityUserRequest newUserRequest);
        IdentityUser RetrieveUserById(int identityUserId);        
        int RetrieveTotalUsers();
        void Update(IdentityUser user);
        void UpdateLastModified(IdentityUser user);
        void Delete(IdentityUser user);        
    }
}

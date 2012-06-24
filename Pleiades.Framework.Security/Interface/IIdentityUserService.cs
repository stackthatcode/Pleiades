using System;
using Pleiades.Framework.Data;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.Identity.Interface
{
    /// <summary>
    /// Contains all CRUD operations for managing Identity Users
    /// </summary>
    public interface IIdentityUserService
    {
        int GetUserCountByRole(UserRole role);
        IdentityUser Create(CreateOrModifyIdentityUserRequest newUserRequest);
        IdentityUser RetrieveUserById(int identityUserId);        
        int RetrieveTotalUsers();
        void Update(CreateOrModifyIdentityUserRequest user);
        void UpdateLastModified(int id);    // Er, can we deprecate this guy?
        void Delete(int id);        
    }
}

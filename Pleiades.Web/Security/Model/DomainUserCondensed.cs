using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Pleiades.Framework.Helpers;

namespace Pleiades.Framework.Web.Security.Model
{
    /// <summary>
    /// This probably belongs in the Domain assembly, as it's very context specific for security stuffs 
    /// </summary>
    public class DomainUserCondensed
    {
        // Membership
        public int DomainUserId { get; set; } 
        public Guid MembershipUserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public bool IsApproved { get; set; }
        public bool IsOnline { get; set; }
        public bool IsLockedOut { get; set; }

        // Authorization
        public UserRole UserRole { get; set; }
        public AccountLevel AccountLevel { get; set; }
        public AccountStatus AccountStatus { get; set; }

        // User Information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime CreationDate { get; set; }

        public string Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public Data.User MembershipUser
        {
            set
            {
                Mapper.CreateMap<Data.User, DomainUserCondensed>();
                Mapper.Map(value, this);
            }
        }

        public Data.DomainUser DomainUser
        {
            set
            {
                Mapper.CreateMap<Data.DomainUser, DomainUserCondensed>();
                Mapper.Map(value, this);

                this.UserRole = value.UserRole.StringToEnum<UserRole>();
                
                // Necessary...?
                this.AccountLevel = (AccountLevel)value.AccountLevel;
            }
        }
    }
}

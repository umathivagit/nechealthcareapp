//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace training.healthcareportal.EDMX
{
    using System;
    using System.Collections.Generic;
    
    public partial class Role
    {
        public Role()
        {
            this.Users = new HashSet<User>();
        }
    
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public byte Active { get; set; }
    
        public virtual ICollection<User> Users { get; set; }
    }
}

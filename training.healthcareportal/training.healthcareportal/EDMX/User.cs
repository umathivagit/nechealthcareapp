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
    
    public partial class User
    {
        public int User_ID { get; set; }
        public string Password { get; set; }
        public int Role_ID { get; set; }
        public byte Active { get; set; }
        public string Username { get; set; }
    
        public virtual Role Role { get; set; }
    }
}

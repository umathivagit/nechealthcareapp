//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace com.necsws.healthcareportal.EDMX
{
    using System;
    using System.Collections.Generic;
    
    public partial class Doctor_Services
    {
        public int Doc_Service_ID { get; set; }
        public int Service_ID { get; set; }
        public int Doctor_ID { get; set; }
    
        public virtual Doctor Doctor { get; set; }
        public virtual Service Service { get; set; }
    }
}

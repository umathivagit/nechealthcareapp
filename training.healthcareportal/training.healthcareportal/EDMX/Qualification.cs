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
    
    public partial class Qualification
    {
        public Qualification()
        {
            this.Doctor_Qualification = new HashSet<Doctor_Qualification>();
        }
    
        public int Qualification_ID { get; set; }
        public string Qualification_Name { get; set; }
    
        public virtual ICollection<Doctor_Qualification> Doctor_Qualification { get; set; }
    }
}
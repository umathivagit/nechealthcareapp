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
    
    public partial class Payment
    {
        public int PaymentID { get; set; }
        public int Patient_ID { get; set; }
        public int ModeOfPayment { get; set; }
        public double Amount { get; set; }
        public string TransactionID { get; set; }
        public System.DateTime Created { get; set; }
    
        public virtual ModeOfPayment ModeOfPayment1 { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
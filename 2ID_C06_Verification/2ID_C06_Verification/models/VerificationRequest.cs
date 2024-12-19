using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2ID_C06_Verification.models
{
    public class VerificationRequest
    {
        public string sodData { get; set; }
        public string dg1DataB64 { get; set; }
        public string dg2DataB64 { get; set; }
        public string dg13DataB64 { get; set; }
        public string dg14DataB64 { get; set; }
        public string method { get; set; }


        public VerificationRequest(string sodData, string dg1DataB64, string dg2DataB64, string dg13DataB64, string dg14DataB64)
        {
            this.sodData = sodData;
            this.dg1DataB64 = dg1DataB64;
            this.dg2DataB64 = dg2DataB64;
            this.dg13DataB64 = dg13DataB64;
            this.dg14DataB64 = dg14DataB64;
        }
    }
}

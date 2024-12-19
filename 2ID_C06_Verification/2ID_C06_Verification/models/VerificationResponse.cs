using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2ID_C06_Verification.models
{
    public class VerificationResponse
    {

        public string signature { get; set; }
        public string responseId { get; set; }
        public int exitCode { get; set; }
        public bool result { get; set; }
        public string message { get; set; }
        public long time { get; set; }

        public VerificationResponse(string signature, string responseId, int exitCode, bool result, string message, long time)
        {
            this.signature = signature;
            this.responseId = responseId;
            this.exitCode = exitCode;
            this.result = result;
            this.message = message;
            this.time = time;
        }
    }
}

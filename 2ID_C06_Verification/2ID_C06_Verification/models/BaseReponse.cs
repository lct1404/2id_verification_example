using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2ID_C06_Verification.models
{
    public class BaseReponse<T>
    {
        public bool status { get; set; }
        public string timestamp { get; set; }
        public string transId { get; set; }
        public T data { get; set; }
    }
}

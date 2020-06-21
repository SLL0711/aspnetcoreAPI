using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Models.CommonModel
{
    public class mJsonResult
    {
        public bool Success { get; set; } = true;
        public string Msg { get; set; }
        public object Rows { get; set; }
        public object Obj { get; set; }
        public int Code { get; set; }
    }
}

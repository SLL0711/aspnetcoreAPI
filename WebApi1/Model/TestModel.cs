using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi1.Model
{
    public class TestModel
    {
        public string className { get; set; }
        public ChildModel childModel { get; set; }
    }

    public class ChildModel
    {
        public string name { get; set; }
        public int age { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace DB.DbModels
{
    public partial class TbPetCategory
    {
        public Guid CategoryId { get; set; }
        public string TypeName { get; set; }
        public DateTime? Createdon { get; set; }
        public string Createdby { get; set; }
        public DateTime? Modifiedon { get; set; }
        public string Modifiedby { get; set; }
        public short? Statecode { get; set; }
    }
}

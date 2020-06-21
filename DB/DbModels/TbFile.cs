using System;
using System.Collections.Generic;

namespace DB.DbModels
{
    public partial class TbFile
    {
        public Guid FileId { get; set; }
        public string FileName { get; set; }
        public int? FileSize { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string DocumentInfo { get; set; }
        public string TbName { get; set; }
        public string TbId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public short? Statecode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrataWork.DTO
{
    public class WorkItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public decimal? WorkDone { get; set; }
        public decimal? WorkRemaining { get; set; }
        public string Attachment { get; set; }
        public string FileName { get; set; }
    }
}
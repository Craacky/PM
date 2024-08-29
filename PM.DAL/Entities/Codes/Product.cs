using PM.DAL.Entities.Base;
using System;

namespace PM.DAL.Entities
{
    public class Product : Entity
    {
        public string MarkingCode { get; set; }
        public Guid ReportTaskGuid { get; set; }


        public int LineId { get; set; }

        public int? BoxId { get; set; }
        public virtual Box Box { get; set; }
    }
}

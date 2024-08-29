using PM.DAL.Entities.Base;
using System;
using System.Collections.Generic;

namespace PM.DAL.Entities
{
    public class Pallet : Entity
    {
        public string MarkingCode { get; set; }
        public bool IsFulled { get; set; }
        public Guid ReportTaskGuid { get; set; }


        public int LineId { get; set; }

        public int ReportTaskId { get; set; }

        public ReportTask ReportTask { get; set; }

        public List<Box> Boxes { get; set; } = new List<Box>();
    }


}

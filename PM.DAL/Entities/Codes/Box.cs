using PM.DAL.Entities.Base;
using System;
using System.Collections.Generic;

namespace PM.DAL.Entities
{
    public class Box : Entity
    {
        public string MarkingCode { get; set; }
        public Guid ReportTaskGuid { get; set; }


        public int LineId { get; set; }

        public int? PalletId { get; set; }
        public Pallet Pallet { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}

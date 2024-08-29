namespace PM.Formats.JSON.Objects.Write
{
    public class TaskMarks
    {
        public int? ExporterCode { get; set; }
        public int Code { get; set; } = 0;
        public string ClaimNumber { get; set; }
        public int? PartnerCode { get; set; } = null;
        public int MarkTemplateCode { get; set; } = 0;
        public int LineCode { get; set; }
        public string StartTime { get; set; }
        public string CloseTime { get; set; }
        public ICollection<Attributes> Attributes { get; set; }
        public ICollection<Barcodes> Barcodes { get; set; }
    }
}

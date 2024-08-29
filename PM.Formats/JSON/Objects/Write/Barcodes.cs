namespace PM.Formats.JSON.Objects.Write
{
    public class Barcodes
    {
        public string Barcode { get; set; }
        public int Level { get; set; }
        public int NumberInTask { get; set; }
        public int? Weight { get; set; } = null;
        public ICollection<Barcodes> ChildBarcodes { get; set; }
    }
}

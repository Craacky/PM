namespace PM.Formats.JSON.Objects.Read
{
    public class Nomenclature
    {
        //Целое, уникальное в рамках экспортёра, код номенклатуры
        public int? Code { get; set; }

        //Целое, код экспортёра, сообщается из АС Маркировки
        public int? ExporterCode { get; set; }

        //Целое, код группы товаров
        public int? GrpCode { get; set; }

        //Текст, наименование товара
        public string Name { get; set; }

        //Текст 14 символов, GTIN товара
        public string Gtin { get; set; }

        //Текст, может быть пусто, артикул товара 
        public int? ArtCode { get; set; }

        //Текст, может быть пусто, описание
        public string Description { get; set; }

        //Список объектов, значения атрибута товара
        public ICollection<Attribute> Attributes { get; set; }
    }
}

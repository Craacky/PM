using PM.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PM.DAL.Entities
{
    public class Nomenclature : Entity
    {
        public Guid Guid { get; set; }


        //Целое, уникальное в рамках экспортёра, код номенклатуры
        public int? Code { get; set; }
        //Целое, код экспортёра, сообщается из АС Маркировки
        public int? ExporterCode { get; set; }
        //Целое, код группы товаров
        public int? GrpCode { get; set; }
        //Текст, наименование товара


        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        //Текст 14 символов, GTIN товара
        private string gtin;
        public string Gtin
        {
            get => gtin;
            set
            {
                gtin = value;
                OnPropertyChanged(nameof(Gtin));
            }
        }


        //Текст, может быть пусто, артикул товара 
        public int? ArtCode { get; set; }
        //Текст, может быть пусто, описание
        public string Description { get; set; }


        //Список объектов, значения атрибута товара
        public virtual List<Attribute> Attributes { get; set; } = new List<Attribute>();


        public override string ToString()
        {
            return Name + " " + Attributes.FirstOrDefault(c=> c.Code == 15)?.Value + " " + Attributes.FirstOrDefault(c => c.Code == 14)?.Value + "\n" + Gtin;
        }
    }
}

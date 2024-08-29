using PM.DAL.Entities.Base;

namespace PM.DAL.Entities
{
    public class Attribute : Entity
    {
        //Code 1 Технологическая инструкция
        //Code 2 Состав
        //Code 3 Жирность
        //Code 4 Белки
        //Code 5 Углеводы
        //Code 6 Энергетическая ценность
        //Code 7 Условия хранения(текстовая строка)
        //Code 8 СТБ/ТУ
        //Code 9 Масса нетто короба
        //Code 10 Масса брутто короба
        //Code 11 Срок годности в днях
        //Code 12 Штрих-код транспортной упаковки
        //Code 13 Краткий код номенклатуры(код из 1С)
        //Code 14 Масса в единице потребительской тары
        //Code 15 Торговая марка
        //Code 16 Масса в кг с учетом плотности
        //Code 17 Температура хранения ОТ
        //Code 18 Температура хранения ДО

        //Целое, код атрибута
        public int? Code { get; set; }
        //Текст, может быть пусто (если это позволяют настройки атрибута), значение атрибута
        public string Value { get; set; }


        public int NomenclatureId { get; set; }
        public virtual Nomenclature Nomenclature { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Attribute other = obj as Attribute;
            return Code == other.Code && Value == other.Value;
        }
    }
}

using System;

namespace PM.DAL.Entities.Base
{
    public class Entity : ObservableObject
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}

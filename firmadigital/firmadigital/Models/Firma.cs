using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace firmadigital.Models
{
    public class Firma
    {
        [PrimaryKey, AutoIncrement]
        public int codigo{ get; set; }

        [MaxLength(70)]
        public string nombre { get; set; }

        [MaxLength(70)]
        public string descripcion { get; set; }

        public byte[] fotofirma { get; set; }
    }
}

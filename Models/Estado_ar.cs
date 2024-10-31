using System;
using System.ComponentModel.DataAnnotations;
using MessagePack;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;
namespace API_TEMPERATURA_MAXIMA.Models
{
    public class Estado_ar
    {
        [Key]
        public int IdEstado_ar { get; set; }
        public int estado{ get; set;}
        public int IdAmbiente { get; set; }
    }
}

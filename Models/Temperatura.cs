using System;
using System.ComponentModel.DataAnnotations;

namespace API_TEMPERATURA_MAXIMA.Models
{
    public class Temperatura
    {
        [Key]
        public int IdTemperatura{get;set;}
        public float temperatura{get;set;}
        public DateTime? HorarioTemp{get;set;} 
        public int IdAmbiente{get;set;}

    }
}

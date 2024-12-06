using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API_TEMPERATURA_MAXIMA.Models;
namespace API_TEMPERATURA_MAXIMA.Models
{
    
    public class MudancaTemp
    {
        [Key]
        public int IdMudancaTemp{get;set;}
        public float temperatura_alterada{get;set;}
        public float temperatura{get;set;}
        public int IdAmbiente{get;set;}
        public int IdFuncionario{get;set;}
        public DateTime? HorarioMudanca{get;set;} 

    }
}

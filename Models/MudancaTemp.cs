using System;
using System.ComponentModel.DataAnnotations;

namespace API_TEMPERATURA_MAXIMA.Models
{
    public class MudancaTemp
    {
        [Key]
        public int IdMudancaTemp{get;set;}
        public float temperatura_alterada{get;set;}
        public float temperatura{get;set;}
        public String? NomeAmbiente{get;set;}
        public int IdAmbiente{get;set;}
        public String? NomeUsuario{get;set;}
        public int IdFuncionario{get;set;}
        public TimeOnly HorarioAlteracao{get;set;}
        public DateOnly DataAlteracao{get;set;}    
    }
}

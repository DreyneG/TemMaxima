using System;
using System.ComponentModel.DataAnnotations;

namespace API_TEMPERATURA_MAXIMA.Models
{
    public class UsuarioAmbiente
    {
        [Key]
        public int IdUsuarioAmbiente{get;set;}
        public int IdAmbiente{get;set;}
        public int IdFuncionario{get;set;}
    }
}

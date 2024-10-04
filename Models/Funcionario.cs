using System;
using System.ComponentModel.DataAnnotations;

namespace API_TEMPERATURA_MAXIMA.Models
{
    public class Funcionario
    {
        [Key]
        public int IdFuncionario{get;set;}
        public String? nome_usuario{get;set;}
        public String? email{get;set;}
        public String? password{get;set;}
        public String? cpf{get;set;}
        public bool type_adm{get;set;}

    }
}

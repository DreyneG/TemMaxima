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
        public String? NomeAmbiente{get => Ambiente?.NomeAmbiente;}
        public int IdAmbiente{get;set;}
        public String? NomeUsuario{get => Funcionario?.nome_usuario;}
        public int IdFuncionario{get;set;}
        public TimeOnly HorarioAlteracao{get;set;} = TimeOnly.FromDateTime(DateTime.Now);
        public DateOnly DataAlteracao{get;set;} = DateOnly.FromDateTime(DateTime.Now);

        [ForeignKey("IdFuncionario")]
        public virtual Funcionario? Funcionario{get; set;}
        public virtual Ambiente? Ambiente{get; set;}
    }
}

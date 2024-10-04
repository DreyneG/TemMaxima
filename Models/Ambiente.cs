using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace API_TEMPERATURA_MAXIMA.Models
{
    public class Ambiente
    {
        [Key]
        public int IdAmbiente { get; set; }
        public String? NomeAmbiente { get; set; }
    }
}

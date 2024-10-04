using System;

namespace API_TEMPERATURA_MAXIMA.Models
{
    public class UsuarioAmbiente
    {
        public int id_UsuAmb{get;set;}
        public String? NomeUsuario{get;set;}
        public int id_ambiente{get;set;}
        public int id_funcionario{get;set;}
        public String? NomeAmbiente{get;set;}
    }
}

using System;
using API_TEMPERATURA_MAXIMA.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API_TEMPERATURA_MAXIMA.Context
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options): base(options){

        }
        public DbSet<Ambiente> Ambientes {get;set;}
        public DbSet<Funcionario> Funcionarios{get;set;}
        public DbSet<Temperatura> Temperaturas{get;set;}
        public DbSet<MudancaTemp> MudancaTemps{get;set;}

        
    }
}

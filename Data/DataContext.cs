using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {

//coasntruccotr cto*->
    public DataContext(DbContextOptions<DataContext> options) :base(options){}
//cosntructor con la llamada a super() de java a la clase

//la referecmai a los POJOS
    public DbSet<Value> Values { get; set; }

//cada POJO lleva un DbSet
    public DbSet<User> Users { get;  set; }
//Se debe crear una nuva migration, es decir una tabla en el 
//contexto usanto ef Entity Framework
//dotnet ef migrations add AddUserEntity  dotnet ef migrations add AddedUserEntity
    }
}
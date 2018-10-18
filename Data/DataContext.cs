using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
public class DataContext : DbContext
 {

//coasntruccotr cto*->
    public DataContext(DbContextOptions<DataContext> options) :base(options){}
//cosntructor con la llamada a super() de java a la clase

    public DbSet<Value> Values { get; set; }
//la referecmai a los POJOS
}
}
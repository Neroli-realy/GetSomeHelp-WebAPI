using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;

namespace GetSomeHelp.Models{
    public class GSHContext : DbContext{
        public GSHContext(DbContextOptions<GSHContext> options) 
        : base(options){ }

        public DbSet<User> User {get; set;}

        public DbSet<Task> Task { get; set; }
        }

    public class GSHContextFactory{

        public static GSHContext Create(string connectionString){
            var options = new DbContextOptionsBuilder<GSHContext>();
            options.UseMySQL(connectionString);
            var dbcontext = new GSHContext(options.Options);
            return dbcontext;
        }
    }
}
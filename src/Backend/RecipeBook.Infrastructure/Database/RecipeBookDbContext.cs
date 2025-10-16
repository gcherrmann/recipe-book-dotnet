using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Infrastructure.Database
{
    public class RecipeBookDbContext: DbContext
    {
        public RecipeBookDbContext(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<Domain.Entities.User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeBookDbContext).Assembly);
        }
    }
}

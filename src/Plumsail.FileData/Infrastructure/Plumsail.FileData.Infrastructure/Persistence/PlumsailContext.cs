using Microsoft.EntityFrameworkCore;
using Plumsail.FileData.Application.Interfaces;
using Plumsail.FileData.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Plumsail.FileData.Infrastructure.Persistence
{
    /// <summary>
    /// Represents the application context derived from <see cref="DbContext"/>.
    /// </summary>
    public class PlumsailContext : DbContext, IApplicationContext
    {
        public PlumsailContext(DbContextOptions<PlumsailContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FileProcessing>();
        }
    }
}

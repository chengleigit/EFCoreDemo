using EFCore.Demo.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Demo.Data
{
    public class DemoContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=127.0.0.1;database=EFCoreDB;uid=sa;pwd=chder@123");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //联合主键
            modelBuilder.Entity<GamePlayer>().HasKey(i => new { i.PlayerId, i.GameId });
            //一对一
            modelBuilder.Entity<Resume>()
                .HasOne(x => x.Player)
                .WithOne(x => x.Resume)
                .HasForeignKey<Resume>(x => x.PlayerId);
                
        }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Player> Players { get; set; }

    }
}

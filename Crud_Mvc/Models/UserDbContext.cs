using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Crud_Mvc.Models
{
    public partial class UserDbContext : DbContext
    {
        public UserDbContext()
            : base("name=UserDbContext")
        {
        }

        public virtual DbSet<catalog> catalogs { get; set; }
        public virtual DbSet<news> news { get; set; }
        public virtual DbSet<skill> skills { get; set; }
        public virtual DbSet<user> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<catalog>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<catalog>()
                .HasMany(e => e.news)
                .WithOptional(e => e.catalog)
                .HasForeignKey(e => e.catalog_id);

            modelBuilder.Entity<news>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<news>()
                .Property(e => e.bref)
                .IsUnicode(false);

            modelBuilder.Entity<news>()
                .Property(e => e.desc)
                .IsUnicode(false);

            modelBuilder.Entity<news>()
                .Property(e => e.photo)
                .IsUnicode(false);

            modelBuilder.Entity<skill>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<skill>()
                .HasMany(e => e.users)
                .WithMany(e => e.skills)
                .Map(m => m.ToTable("skill_user"));

            modelBuilder.Entity<user>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.photo)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.cv)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.news)
                .WithOptional(e => e.user)
                .HasForeignKey(e => e.user_id);
        }
    }
}

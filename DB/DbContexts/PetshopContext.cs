using System;
using DB.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DB.DbContexts
{
    public partial class PetshopContext : DbContext
    {
        public PetshopContext()
        {
        }

        public PetshopContext(DbContextOptions<PetshopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbFile> TbFile { get; set; }
        public virtual DbSet<TbPetCategory> TbPetCategory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbFile>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.ToTable("tb_File");

                entity.Property(e => e.FileId).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.FileName).HasMaxLength(50);

                entity.Property(e => e.FilePath).HasMaxLength(200);

                entity.Property(e => e.FileType).HasMaxLength(20);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.TbId)
                    .HasColumnName("Tb_Id")
                    .HasMaxLength(50);

                entity.Property(e => e.TbName)
                    .HasColumnName("Tb_Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TbPetCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.ToTable("tb_PetCategory");

                entity.Property(e => e.CategoryId).ValueGeneratedNever();

                entity.Property(e => e.Createdby)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Createdon).HasColumnType("datetime");

                entity.Property(e => e.Modifiedby).HasMaxLength(50);

                entity.Property(e => e.Modifiedon).HasColumnType("datetime");

                entity.Property(e => e.TypeName).HasMaxLength(50);
            });
        }
    }
}

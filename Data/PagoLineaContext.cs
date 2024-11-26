using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CAEV.PagoLinea.Models;

namespace CAEV.PagoLinea.Data
{
    public class PagoLineaContext : DbContext {

        public DbSet<CuentaPadron> CuentasPadron {get;set;} = default!;
        public DbSet<CatOficina> Oficinas {get;set;} = default!;
        public DbSet<CatLocalidad> Localidades {get;set;} = default!;
        public DbSet<User> Users {get;set;} = default!;
        public DbSet<OrderPayment> OrdersPayment {get;set;} = default!;
        public DbSet<SystemOption> SystemOptions {get;set;} = default!;

        public PagoLineaContext(DbContextOptions options ) : base(options){
            //
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // * Convert all columns in comel case
            foreach( var entity in modelBuilder.Model.GetEntityTypes() )
            {
                foreach( var property in entity.GetProperties() )
                {
                    var _propertyName = property.Name;
                    property.SetColumnName( Char.ToLowerInvariant(_propertyName[0]) + _propertyName.Substring(1) );
                }
            }

            // * CuentaPadron entity
            modelBuilder.Entity<CuentaPadron>( entity =>
            {
                entity.Property(p => p.CreatedAt)
                    .HasDefaultValueSql("getDate()")
                    .HasColumnType("datetime");

                entity.Property(p => p.UpdatedAt)
                    .HasDefaultValueSql("getDate()")
                    .HasColumnType("datetime");

                entity.HasOne(item => item.Oficina)
                    .WithMany()
                    .HasForeignKey("oficinaId");
            });


            // * CatLocalidad Entity
            modelBuilder.Entity<CatLocalidad>()
            .HasOne(l => l.Oficina) // CatLocalidad has one Oficina
            .WithMany(o => o.Localidades) // CatOficina has many Localidades (assuming this navigation exists in CatOficina)
            .HasForeignKey(l => l.OficinaId); // The foreign key is OficinaId


            // * User entity
            var userEntity = modelBuilder.Entity<User>();
            userEntity.Property( p => p.CreatedAt)
                .HasDefaultValueSql("getDate()")
                .HasColumnType("datetime");
            userEntity.Property( p => p.UpdatedAt)
                .HasDefaultValueSql("getDate()")
                .HasColumnType("datetime");

            // * OrderPayment entity
            var orderPayment = modelBuilder.Entity<OrderPayment>();
            userEntity.Property( p => p.CreatedAt)
                .HasDefaultValueSql("getDate()")
                .HasColumnType("datetime2")
                .ValueGeneratedOnAdd();
            userEntity.Property( p => p.UpdatedAt)
                .HasColumnType("datetime2");

            
            // * default user
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123#");

            // Seed a default user
            modelBuilder.Entity<User>().HasData( new User
                {
                    Id = 1, // Set the ID manually
                    Name = "Administrador",
                    Email = "admin@email.com",
                    Password = hashedPassword
                }
            );

            modelBuilder.Entity<SystemOption>().HasData(new SystemOption {
                Id = 1,
                Key = "ON-MAINTENANCE",
                Value = "0"
            });

            base.OnModelCreating(modelBuilder);
            
        }

    }
}
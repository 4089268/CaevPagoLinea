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
            var cuentaPadronEntity = modelBuilder.Entity<CuentaPadron>();
            cuentaPadronEntity.Property( p => p.CreatedAt)
                .HasDefaultValueSql("getDate()")
                .HasColumnType("datetime");
            cuentaPadronEntity.Property( p => p.UpdatedAt)
                .HasDefaultValueSql("getDate()")
                .HasColumnType("datetime")
                .ValueGeneratedOnAddOrUpdate();

            // * CatLocalidad Entity
            modelBuilder.Entity<CatLocalidad>()
            .HasOne(l => l.Oficina) // CatLocalidad has one Oficina
            .WithMany(o => o.Localidades) // CatOficina has many Localidades (assuming this navigation exists in CatOficina)
            .HasForeignKey(l => l.OficinaId); // The foreign key is OficinaId

            base.OnModelCreating(modelBuilder);
            
        }

    }
}
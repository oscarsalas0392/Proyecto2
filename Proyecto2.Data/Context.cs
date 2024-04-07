﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Proyecto2.Model;

namespace Proyecto2.Data;

public partial class Context : DbContext
{
    public Context(DbContextOptions<Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Artista> Artista { get; set; }

    public virtual DbSet<CategoriaObra> CategoriaObra { get; set; }

    public virtual DbSet<ConfiguracionApp> ConfiguracionApp { get; set; }

    public virtual DbSet<DimensionObra> DimensionObra { get; set; }

    public virtual DbSet<ImagenObra> ImagenObra { get; set; }

    public virtual DbSet<Mensaje> Mensaje { get; set; }

    public virtual DbSet<Notificacion> Notificacion { get; set; }

    public virtual DbSet<ObraArte> ObraArte { get; set; }

    public virtual DbSet<Oferta> Oferta { get; set; }

    public virtual DbSet<Subasta> Subasta { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }

    public virtual DbSet<Transaccion> Transaccion { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artista>(entity =>
        {
            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Artista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Artista_Usuario");
        });

        modelBuilder.Entity<ConfiguracionApp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_configuracionApp");
        });

        modelBuilder.Entity<DimensionObra>(entity =>
        {
            entity.HasOne(d => d.ObraArteNavigation).WithMany(p => p.DimensionObra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DimensionObra_ObraArte");
        });

        modelBuilder.Entity<ImagenObra>(entity =>
        {
            entity.HasOne(d => d.ObraArteNavigation).WithMany(p => p.ImagenObra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImagenObra_ObraArte");
        });

        modelBuilder.Entity<Mensaje>(entity =>
        {
            entity.HasOne(d => d.EmisorNavigation).WithMany(p => p.Mensaje).HasConstraintName("FK_Mensaje_Emisor");

            entity.HasOne(d => d.ReceptorNavigation).WithMany(p => p.Mensaje).HasConstraintName("FK_Mensaje_Receptor");

            entity.HasOne(d => d.SubastaNavigation).WithMany(p => p.Mensaje)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mensaje_Subasta");
        });

        modelBuilder.Entity<ObraArte>(entity =>
        {
            entity.HasOne(d => d.ArtistaNavigation).WithMany(p => p.ObraArte)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ObraArte_Artista");

            entity.HasOne(d => d.CategoriaObraNavigation).WithMany(p => p.ObraArte)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ObraArte_Categoria");
        });

        modelBuilder.Entity<Oferta>(entity =>
        {
            entity.HasOne(d => d.SubastaNavigation).WithMany(p => p.Oferta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Oferta_Subasta");

            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Oferta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Oferta_Usuario");
        });

        modelBuilder.Entity<Subasta>(entity =>
        {
            entity.HasOne(d => d.ObraArteNavigation).WithMany(p => p.Subasta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subasta_ObraArte");
        });

        modelBuilder.Entity<Transaccion>(entity =>
        {
            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Transaccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaccion_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasOne(d => d.TipoUsuarioNavigation).WithMany(p => p.Usuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_TipoUsuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EcoNet.Models
{
    public partial class EcoNetContext : DbContext
    {
        public EcoNetContext()
        {
        }

        public EcoNetContext(DbContextOptions<EcoNetContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Anuncio> Anuncios { get; set; } = null!;
        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<Empresa> Empresas { get; set; } = null!;
        public virtual DbSet<EtiquetaAnuncio> EtiquetaAnuncios { get; set; } = null!;
        public virtual DbSet<Etiqueta> Etiqueta { get; set; } = null!;
        public virtual DbSet<Mensaje> Mensajes { get; set; } = null!;
        public virtual DbSet<Oferta> Oferta { get; set; } = null!;
        public virtual DbSet<Particular> Particulars { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<Venta> Venta { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=200.234.224.123,54321;Initial Catalog=EcoNet;User ID=sa;Password=Sql#123456789;Encrypt=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anuncio>(entity =>
            {
                entity.HasKey(e => e.IdAnuncio)
                    .HasName("PK__Anuncio__BD6A762208B72751");

                entity.ToTable("Anuncio");

                entity.Property(e => e.IdAnuncio).ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FkborradoPor).HasColumnName("FKBorradoPor");

                entity.Property(e => e.Fkusuario).HasColumnName("FKUsuario");

                entity.Property(e => e.Precio).HasColumnType("money");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkborradoPorNavigation)
                    .WithMany(p => p.AnuncioFkborradoPorNavigations)
                    .HasForeignKey(d => d.FkborradoPor)
                    .HasConstraintName("FKAnuncioUsuarioAdmin");

                entity.HasOne(d => d.FkusuarioNavigation)
                    .WithMany(p => p.AnuncioFkusuarioNavigations)
                    .HasForeignKey(d => d.Fkusuario)
                    .HasConstraintName("FKAnuncioUsuario");
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.HasKey(e => e.IdChat)
                    .HasName("PK__Chat__3817F38C74FE75B8");

                entity.ToTable("Chat");

                entity.Property(e => e.IdChat).ValueGeneratedNever();

                entity.Property(e => e.Fkanuncio).HasColumnName("FKAnuncio");

                entity.Property(e => e.Fkcomprador).HasColumnName("FKComprador");

                entity.Property(e => e.Fkvendedor).HasColumnName("FKVendedor");

                entity.HasOne(d => d.FkanuncioNavigation)
                    .WithMany(p => p.Chats)
                    .HasForeignKey(d => d.Fkanuncio)
                    .HasConstraintName("FKChatAnuncio");

                entity.HasOne(d => d.FkcompradorNavigation)
                    .WithMany(p => p.ChatFkcompradorNavigations)
                    .HasForeignKey(d => d.Fkcomprador)
                    .HasConstraintName("FKChatComprador");

                entity.HasOne(d => d.FkvendedorNavigation)
                    .WithMany(p => p.ChatFkvendedorNavigations)
                    .HasForeignKey(d => d.Fkvendedor)
                    .HasConstraintName("FKChatVendedor");
            });

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Empresa__5B65BF97F1E923C0");

                entity.ToTable("Empresa");

                entity.Property(e => e.IdUsuario).ValueGeneratedNever();

                entity.Property(e => e.Cif)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithOne(p => p.Empresa)
                    .HasForeignKey<Empresa>(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Empresa__IdUsuar__3C69FB99");
            });

            modelBuilder.Entity<EtiquetaAnuncio>(entity =>
            {
                entity.HasKey(e => e.IdEtiquetaAnuncio);

                entity.ToTable("EtiquetaAnuncio");

                entity.Property(e => e.Fkanuncio).HasColumnName("FKAnuncio");

                entity.Property(e => e.Fketiqueta).HasColumnName("FKEtiqueta");

                entity.HasOne(d => d.FkanuncioNavigation)
                    .WithMany(p => p.EtiquetaAnuncios)
                    .HasForeignKey(d => d.Fkanuncio)
                    .HasConstraintName("FK_EtiquetaAnuncio_Anuncio");

                entity.HasOne(d => d.FketiquetaNavigation)
                    .WithMany(p => p.EtiquetaAnuncios)
                    .HasForeignKey(d => d.Fketiqueta)
                    .HasConstraintName("FK_EtiquetaAnuncio_Etiqueta");
            });

            modelBuilder.Entity<Etiqueta>(entity =>
            {
                entity.HasKey(e => e.IdEtiqueta);

                entity.Property(e => e.DescripcionEtiqueta)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Mensaje>(entity =>
            {
                entity.HasKey(e => e.IdMensaje)
                    .HasName("PK__Mensaje__E4D2A47F1B0EBD45");

                entity.ToTable("Mensaje");

                entity.Property(e => e.IdMensaje).ValueGeneratedNever();

                entity.Property(e => e.Fkchat).HasColumnName("FKChat");

                entity.Property(e => e.HoraMensaje).HasColumnType("datetime");

                entity.Property(e => e.Texto)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkchatNavigation)
                    .WithMany(p => p.Mensajes)
                    .HasForeignKey(d => d.Fkchat)
                    .HasConstraintName("FKMensajeChat");
            });

            modelBuilder.Entity<Oferta>(entity =>
            {
                entity.HasKey(e => e.IdOferta)
                    .HasName("PK__Oferta__5420E1DA0CEFE8D6");

                entity.Property(e => e.IdOferta).ValueGeneratedNever();

                entity.Property(e => e.Fkchat).HasColumnName("FKChat");

                entity.Property(e => e.Precio).HasColumnType("money");

                entity.HasOne(d => d.FkchatNavigation)
                    .WithMany(p => p.Oferta)
                    .HasForeignKey(d => d.Fkchat)
                    .HasConstraintName("FK_Oferta_Chat");
            });

            modelBuilder.Entity<Particular>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Particul__5B65BF9730E743C9");

                entity.ToTable("Particular");

                entity.Property(e => e.IdUsuario).ValueGeneratedNever();

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Dni)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithOne(p => p.Particular)
                    .HasForeignKey<Particular>(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Particula__IdUsu__3F466844");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuario__5B65BF97ED5EF1B5");

                entity.ToTable("Usuario");

                entity.Property(e => e.IdUsuario).ValueGeneratedNever();

                entity.Property(e => e.Contraseña)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAlta).HasColumnType("date");

                entity.Property(e => e.FechaBaja).HasColumnType("date");

                entity.Property(e => e.Municipio)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario1)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Usuario");
            });

            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasKey(e => e.IdVenta)
                    .HasName("PK__Venta__BC1240BD15F0192A");

                entity.Property(e => e.IdVenta).ValueGeneratedNever();

                entity.Property(e => e.Fkoferta).HasColumnName("FKOferta");

                entity.Property(e => e.Precio).HasColumnType("decimal(18, 0)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

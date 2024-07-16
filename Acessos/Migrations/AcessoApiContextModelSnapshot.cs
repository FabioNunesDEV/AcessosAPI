﻿// <auto-generated />
using System;
using Acessos.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Acessos.Migrations
{
    [DbContext(typeof(AcessoApiContext))]
    partial class AcessoApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Acessos.Models.Grupo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.ToTable("Grupo");
                });

            modelBuilder.Entity("Acessos.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("Acessos.Models.UsuarioGrupo", b =>
                {
                    b.Property<int?>("UsuarioId")
                        .HasColumnType("int");

                    b.Property<int?>("GrupoId")
                        .HasColumnType("int");

                    b.HasKey("UsuarioId", "GrupoId");

                    b.HasIndex("GrupoId");

                    b.ToTable("UsuarioGrupo");
                });

            modelBuilder.Entity("Acessos.Models.UsuarioGrupo", b =>
                {
                    b.HasOne("Acessos.Models.Grupo", "Grupo")
                        .WithMany("UsuarioGrupos")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Acessos.Models.Usuario", "Usuario")
                        .WithMany("UsuarioGrupos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grupo");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Acessos.Models.Grupo", b =>
                {
                    b.Navigation("UsuarioGrupos");
                });

            modelBuilder.Entity("Acessos.Models.Usuario", b =>
                {
                    b.Navigation("UsuarioGrupos");
                });
#pragma warning restore 612, 618
        }
    }
}

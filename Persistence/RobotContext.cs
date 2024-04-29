using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

public partial class RobotContext : DbContext
{
    public RobotContext()
    {
    }

    public RobotContext(DbContextOptions<RobotContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Map> Maps { get; set; }

    public virtual DbSet<RobotCommand> RobotCommands { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=password;Database=postgres").LogTo(Console.Write).EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("pg_catalog", "adminpack")
            .HasPostgresExtension("pgagent", "pgagent");

        modelBuilder.Entity<Map>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_map");

            entity.ToTable("map");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Columns).HasColumnName("columns");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Description)
                .HasMaxLength(800)
                .HasColumnName("description");
            entity.Property(e => e.IsSquare)
                .HasComputedColumnSql("((rows > 0) AND (rows = columns))", true)
                .HasColumnName("issquare");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Rows).HasColumnName("rows");
        });

        modelBuilder.Entity<RobotCommand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_robotcommand");
            entity.ToTable("robotcommand");
            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e =>
                e.IsMoveCommand).HasColumnName("ismovecommand");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("Name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

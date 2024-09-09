using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GameCriticBL.Models;

public partial class RwaprojectDbContext : DbContext
{
    public RwaprojectDbContext()
    {
    }

    public RwaprojectDbContext(DbContextOptions<RwaprojectDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameGenre> GameGenres { get; set; }

    public virtual DbSet<GameType> GameTypes { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<LogItem> LogItems { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<UserGamer> UserGamers { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Idgame).HasName("PK__Game__D0B765D547D652A2");

            entity.ToTable("Game");

            entity.Property(e => e.Idgame).HasColumnName("IDGame");
            entity.Property(e => e.Description).HasMaxLength(150);
            entity.Property(e => e.GameName).HasMaxLength(150);
            entity.Property(e => e.GameTypeId).HasColumnName("GameTypeID");

            entity.HasOne(d => d.GameType).WithMany(p => p.Games)
                .HasForeignKey(d => d.GameTypeId)
                .HasConstraintName("FK__Game__GameTypeID__5EBF139D");
        });

        modelBuilder.Entity<GameGenre>(entity =>
        {
            entity.HasKey(e => e.IdgameGenre).HasName("PK__GameGenr__6CF53E8DC5674E6D");

            entity.ToTable("GameGenre");

            entity.Property(e => e.IdgameGenre).HasColumnName("IDGameGenre");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.GenreId).HasColumnName("GenreID");

            entity.HasOne(d => d.Game).WithMany(p => p.GameGenres)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__GameGenre__GameI__6383C8BA");

            entity.HasOne(d => d.Genre).WithMany(p => p.GameGenres)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK__GameGenre__Genre__6477ECF3");
        });

        modelBuilder.Entity<GameType>(entity =>
        {
            entity.HasKey(e => e.IdgameType).HasName("PK__GameType__005B47BCF8582AF6");

            entity.ToTable("GameType");

            entity.Property(e => e.IdgameType).HasColumnName("IDGameType");
            entity.Property(e => e.Description).HasMaxLength(150);
            entity.Property(e => e.GameTypeName).HasMaxLength(150);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Idgenre).HasName("PK__Genre__23FDA2F0356B6691");

            entity.ToTable("Genre");

            entity.Property(e => e.Idgenre).HasColumnName("IDGenre");
            entity.Property(e => e.Description).HasMaxLength(150);
            entity.Property(e => e.GenreName).HasMaxLength(150);
        });

        modelBuilder.Entity<LogItem>(entity =>
        {
            entity.HasKey(e => e.Idlog).HasName("PK__LogItem__95D002085612139E");

            entity.ToTable("LogItem");

            entity.Property(e => e.Idlog).HasColumnName("IDLog");
            entity.Property(e => e.Tmstamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Idreview).HasName("PK__Review__E5003B6F9DFC4D51");

            entity.ToTable("Review");

            entity.Property(e => e.Idreview).HasColumnName("IDReview");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.GamerId).HasColumnName("GamerID");

            entity.HasOne(d => d.Game).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__Review__GameID__693CA210");

            entity.HasOne(d => d.Gamer).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.GamerId)
                .HasConstraintName("FK__Review__GamerID__6A30C649");
        });

        modelBuilder.Entity<UserGamer>(entity =>
        {
            entity.HasKey(e => e.IduserGamer).HasName("PK__UserGame__0FB14DF6FB75BFDB");

            entity.ToTable("UserGamer");

            entity.Property(e => e.IduserGamer).HasColumnName("IDUserGamer");
            entity.Property(e => e.City).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FirstName).HasMaxLength(150);
            entity.Property(e => e.HomeAddress).HasMaxLength(150);
            entity.Property(e => e.LastName).HasMaxLength(150);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.PwdHash).HasMaxLength(256);
            entity.Property(e => e.PwdSalt).HasMaxLength(256);
            entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");
            entity.Property(e => e.Username).HasMaxLength(100);

            entity.HasOne(d => d.UserRole).WithMany(p => p.UserGamers)
                .HasForeignKey(d => d.UserRoleId)
                .HasConstraintName("FK__UserGamer__UserR__29221CFB");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.IduserRole).HasName("PK__UserRole__5A7AF781FCF28C90");

            entity.ToTable("UserRole");

            entity.Property(e => e.IduserRole).HasColumnName("IDUserRole");
            entity.Property(e => e.RoleName).HasMaxLength(150);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

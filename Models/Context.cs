using Microsoft.EntityFrameworkCore;

namespace exemplu.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        public DbSet<CONCURS> CONCURSURI { get; set; } = null!;
        public DbSet<CONCURENT> CONCURENTI { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CONCURS>(entity =>
            {
                entity.ToTable("CONCURSURI");
                entity.HasKey(entity => entity.Id);
                entity.Property(entity => entity.Id).HasColumnType("int").ValueGeneratedOnAdd();
                entity.Property(entity => entity.Nume).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(entity => entity.Data).HasColumnType("datetime").IsRequired();
                entity.Property(entity => entity.Categorie).HasColumnType("nvarchar(50)").IsRequired();
                entity.Property(entity => entity.nr_max_participanti).HasColumnType("int").IsRequired();
                entity.Property(entity => entity.restrictie_varsta).HasColumnType("bit").IsRequired();


            });

            modelBuilder.Entity<CONCURENT>(entity =>
            {
                entity.ToTable("CONCURENTI");
                entity.HasKey(entity => entity.Id);
                entity.Property(entity => entity.Id).HasColumnType("int").ValueGeneratedOnAdd();
                entity.Property(entity => entity.Nume).HasColumnType("nvarchar(50)").IsRequired();
                entity.Property(entity => entity.Prenume).HasColumnType("nvarchar(50)").IsRequired();
                entity.Property(entity => entity.DataNasterii).HasColumnType("datetime").IsRequired();
                entity.Property(entity => entity.Tara).HasColumnType("nvarchar(50)").IsRequired();
                entity.Property(entity => entity.CONCURSId).HasColumnType("int").IsRequired();
                entity.Property(entity => entity.Varsta).HasColumnType("int").IsRequired();
                
                
                // Foreign key relationship
                entity.HasOne(e => e.CONCURS)
                    .WithMany(c => c.CONCURENTI)
                    .HasForeignKey(e => e.CONCURSId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<CONCURENT>().ToTable("CONCURENTI");
        }
    }
    
}

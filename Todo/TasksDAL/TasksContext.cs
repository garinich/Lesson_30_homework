using Microsoft.EntityFrameworkCore;

namespace TasksDAL
{
    public class TasksContext : DbContext
    {
        public TasksContext(DbContextOptions<TasksContext> options)
            : base(options)
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<DoneTask> DoneTasks { get; set; }
        public virtual DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("tasks");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IsDone).HasColumnName("isdone").HasDefaultValue(false);
            });

            modelBuilder.Entity<DoneTask>(entity =>
            {
                entity.ToTable("donetasks");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<TaskCategory>()
                .HasKey(t => new {t.TaskId, t.CategoryId});

            modelBuilder.Entity<TaskCategory>()
                .HasOne(sc => sc.Task)
                .WithMany(s => s.TaskCategories)
                .HasForeignKey(sc => sc.TaskId);

            modelBuilder.Entity<TaskCategory>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.TaskCategories)
                .HasForeignKey(sc => sc.CategoryId);


            modelBuilder.Entity<DoneTaskCategory>()
                .HasKey(t => new {t.DoneTaskId, t.CategoryId});

            modelBuilder.Entity<DoneTaskCategory>()
                .HasOne(sc => sc.DoneTask)
                .WithMany(s => s.DoneTaskCategories)
                .HasForeignKey(sc => sc.DoneTaskId);

            modelBuilder.Entity<DoneTaskCategory>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.DoneTaskCategories)
                .HasForeignKey(sc => sc.CategoryId);
        }
    }
}

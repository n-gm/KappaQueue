using KappaQueue.Models.Positions;
using KappaQueue.Models.Queues;
using KappaQueue.Models.Rooms;
using KappaQueue.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace KappaQueue.Models.Context
{
    public class QueueDBContext : DbContext
    {
        public const string POSTGRESQL = "PGSQL";
        public const string MSSQL = "MSSQL";

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Queue> Queues { get; set; }
        public DbSet<Position> Positions { get; set; }
                
        public QueueDBContext(DbContextOptions<QueueDBContext> options)
            : base(options)
        {
            if (Database.EnsureCreated())
            {
                User.AfterSeed(this);
            }            
        }

        /// <summary>
        /// Инициализируем модели при создании структур
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            /*   builder.Entity<User>()
                   .HasIndex(u => u.Username)
                   .IsUnique();

               builder.Entity<User>()
                   .HasIndex(u => u.EntityGuid)
                   .IsUnique();

               builder.Entity<UserRole>()
                   .HasIndex(ur => ur.Code)
                   .IsUnique(); */
            builder
                .Entity<QueueNode>()
                .HasKey(qn => new { qn.QueueId, qn.PositionId })
                .HasName("PK_queue_node");

            //Создаем связь между ролями и пользователями
            builder
                .Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(ur => ur.Users)
                .UsingEntity(t => t.ToTable("user_roles"));

            //Создаем связь между кабинетами и их типами
            builder
                .Entity<RoomType>()
                .HasMany(rt => rt.Rooms)
                .WithOne(r => r.RoomType)
                .HasForeignKey("RoomTypeId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            //Создаем связь между кабинетами и пользователями
            builder
                .Entity<User>()
                .HasMany(u => u.Rooms)
                .WithMany(r => r.Users)
                .UsingEntity(t => t.ToTable("user_rooms"));

            builder
                .Entity<User>()
                .HasMany(u => u.Positions)
                .WithMany(p => p.Users)
                .UsingEntity(t => t.ToTable("user_positions"));

            builder
                .Entity<Position>()
                .HasMany(p => p.QueueNodes)
                .WithOne(qn => qn.Position)
                .HasForeignKey("PositionId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Queue>()
                .HasMany(q => q.QueueNodes)
                .WithOne(qn => qn.Queue)
                .HasForeignKey("QueueId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            //Инициализируем роли
            builder.Entity<UserRole>()
                .HasData(UserRole.Seed());
            
            //Инициализируем основных пользователей
            builder.Entity<User>()
                .HasData(User.Seed());         
        }
    }
}

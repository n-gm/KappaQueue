using KappaQueueCommon.Models.Positions;
using KappaQueueCommon.Models.Queues;
using KappaQueueCommon.Models.Rooms;
using KappaQueueCommon.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace KappaQueueCommon.Models.Context
{
    public class QueueDBContext : DbContext
    {
        public const string POSTGRESQL = "PGSQL";
        public const string MSSQL = "MSSQL";
        public const string SQLITE = "SQLITE";

        /// <summary>
        /// Роли пользователя
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }
        /// <summary>
        /// Список пользователей
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// Список комнат
        /// </summary>
        public DbSet<Room> Rooms { get; set; }
        /// <summary>
        /// Список типов комнат
        /// </summary>
        public DbSet<RoomType> RoomTypes { get; set; }
        /// <summary>
        /// Список групп очередей
        /// </summary>
        public DbSet<QueueGroup> QueueGroups { get; set; }
        /// <summary>
        /// Список очередей
        /// </summary>
        public DbSet<Queue> Queues { get; set; }
        /// <summary>
        /// Этапы очереди
        /// </summary>
        public DbSet<QueueNode> QueueNodes { get; set; }
        /// <summary>
        /// Список должностей
        /// </summary>
        public DbSet<Position> Positions { get; set; }
                
        public QueueDBContext(DbContextOptions<QueueDBContext> options)
            : base(options)
        {
#if DEBUG
            Database.EnsureDeleted();
#endif
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

            builder
                .Entity<QueueGroup>()
                .HasMany(qg => qg.Queues)
                .WithOne(q => q.QueuesGroup)
                .HasForeignKey("QueueGroupId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            //Инициализируем роли
            builder.Entity<UserRole>()
                .HasData(UserRole.Seed());
            
            //Инициализируем основных пользователей
            builder.Entity<User>()
                .HasData(User.Seed());

            //Инициализируем группы
            builder.Entity<QueueGroup>()
                .HasData(QueueGroup.Seed());
        }
    }
}

using KappaQueueCommon.Models.Clients;
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
        public DbSet<QueueStage> QueueNodes { get; set; }
        /// <summary>
        /// Список должностей
        /// </summary>
        public DbSet<Position> Positions { get; set; }
        /// <summary>
        /// Список клиентов
        /// </summary>
        public DbSet<Client> Clients { get; set; }
        /// <summary>
        /// Список состояний клиента
        /// </summary>
        public DbSet<ClientState> ClientStates { get; set; }
        /// <summary>
        /// Список этапов очереди клиента
        /// </summary>
        public DbSet<ClientStage> ClientStages { get; set; }
        /// <summary>
        /// Журналы пользователей
        /// </summary>
        public DbSet<UserJournal> UserJournals { get; set; }
        /// <summary>
        /// Состояния пользователей
        /// </summary>
        public DbSet<UserState> UserStates { get; set; }
        /// <summary>
        /// Список статусов пользователя
        /// </summary>
        public DbSet<UserStatus> UserStatuses { get; set; }
                
        public QueueDBContext(DbContextOptions<QueueDBContext> options)
            : base(options)
        {
#if DEBUG
            Database.EnsureDeleted();
#endif
            if (Database.EnsureCreated())
            {
                User.AfterSeed(this);
                UserRight.AfterSeed(this);
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
            
            CreateKeys(builder);
            CreateRelations(builder);
            Seed(builder);
            
        }

        /// <summary>
        /// Заполнение БД первичными данными
        /// </summary>
        /// <param name="builder"></param>
        private void Seed(ModelBuilder builder)
        {
            //Инициализируем роли
            builder.Entity<UserRole>()
                .HasData(UserRole.Seed());

            //Инициализируем основных пользователей
            builder.Entity<User>()
                .HasData(User.Seed());

            //Инициализируем группы
            builder.Entity<QueueGroup>()
                .HasData(QueueGroup.Seed());
            
            //Инициализируем состояния клиента
            builder.Entity<ClientState>()
                .HasData(ClientState.Seed());

            builder.Entity<UserState>()
                .HasData(UserState.Seed());

            builder.Entity<UserRight>()
                .HasData(UserRight.Seed());
        }

        /// <summary>
        /// Создание первичных ключей
        /// </summary>
        /// <param name="builder"></param>
        private void CreateKeys(ModelBuilder builder)
        {
            builder
               .Entity<QueueStage>()
               .HasKey(qn => new { qn.QueueId, qn.PositionId })
               .HasName("PK_queue_stages");

            builder
                .Entity<ClientStage>()
                .HasKey(cs => new { cs.ClientId, cs.PositionId, cs.Priority })
                .HasName("PK_client_stages");

            builder
                .Entity<ClientStageAssignement>()
                .HasKey(csa => new { csa.ClientId, csa.PositionId, csa.Priority, csa.UserId })
                .HasName("PK_clientStagesAssignement");

            builder
                .Entity<UserJournal>()
                .HasKey(uj => new { uj.OpTime, uj.UserId })
                .HasName("PK_userJournal");
        }
        /// <summary>
        /// Создание связей между таблицами
        /// </summary>
        /// <param name="builder"></param>
        private void CreateRelations(ModelBuilder builder)
        {
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
                .Entity<UserStatus>()
                .HasOne(us => us.User)
                .WithOne(u => u.Status)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<User>()
                .HasMany(u => u.Positions)
                .WithMany(p => p.Users)
                .UsingEntity(t => t.ToTable("user_positions"));

            builder
                .Entity<UserRole>()
                .HasMany(ur => ur.UserRights)
                .WithMany(ur => ur.UserRoles)
                .UsingEntity(t => t.ToTable("roles_rights"));

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

            builder
                .Entity<ClientState>()
                .HasMany(cs => cs.Clients)
                .WithOne(c => c.State)
                .HasForeignKey("StateId")
                .HasConstraintName("FK_clients_clientStates")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Client>()
                .HasMany(cs => cs.ClientStages)
                .WithOne(c => c.Client)
                .HasForeignKey("ClientId")
                .HasConstraintName("FK_clientStages_clients")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Position>()
                .HasMany(p => p.ClientStages)
                .WithOne(cs => cs.Position)
                .HasForeignKey(fk => new { fk.PositionId })
                .HasConstraintName("FK_clientStages_positions")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<ClientStage>()
                .HasMany(cs => cs.Assignements)
                .WithOne(csa => csa.ClientStage)
                .HasForeignKey(fk => new { fk.ClientId, fk.PositionId, fk.Priority })
                .HasConstraintName("FK_clientStagesAssignement_clientStages")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<User>()
                .HasMany(u => u.Assignements)
                .WithOne(csa => csa.User)
                .HasForeignKey(fk => new { fk.UserId })
                .HasConstraintName("FK_clientStagesAssignement_users")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

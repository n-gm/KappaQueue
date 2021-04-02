using KappaQueueCommon.Common.References;
using KappaQueueCommon.Models.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace KappaQueueCommon.Models.Users
{
    [Table("user_rights")]
    [Index("Code", IsUnique = true)]
    public class UserRight
    {
        [Column("id")]
        [Key]
        public byte Id { get; set; }

        [Column("code")]
        [Required]
        [MaxLength(32)]
        public string Code { get; set; }

        [Column("name")]
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Column("description")]
        [MaxLength(256)]
        public string Description { get; set; }

        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public static UserRight[] Seed()
        {
            return new UserRight[]
            {
                new UserRight { Id = 1, Code = RightsRef.ALL_USERS, Name = "Пользователи", Description = "Полный доступ к управлению пользователями" },
                new UserRight { Id = 2, Code = RightsRef.GET_USERS, Name = "Просмотр пользователей", Description = "Просмотр всех пользователей"},
                new UserRight { Id = 3, Code = RightsRef.GET_USER, Name = "Просмотр пользователя", Description = "Посмотр отдельных пользователей"},
                new UserRight { Id = 4, Code = RightsRef.CREATE_USER, Name = "Создание пользователя", Description = "Создание пользователей"},
                new UserRight { Id = 5, Code = RightsRef.CHANGE_USER, Name = "Изменение пользователя", Description = "Изменение пользователя"},
                new UserRight { Id = 6, Code = RightsRef.DELETE_USER, Name = "Блокировка пользователя", Description = "Блокировка пользователя"},

                new UserRight { Id = 7, Code = RightsRef.ALL_POSITIONS, Name = "Должности", Description = "Полный доступ к должностям"},
                new UserRight { Id = 8, Code = RightsRef.GET_POSITIONS, Name = "Посмотр должностей", Description = "Просмотр всех должностей"},
                new UserRight { Id = 9, Code = RightsRef.GET_POSITION, Name = "Просмотр должности", Description = "Просмотр отдельной должности"},
                new UserRight { Id = 10, Code = RightsRef.CREATE_POSITION, Name = "Создание должности", Description = "Создание должности"},
                new UserRight { Id = 11, Code = RightsRef.CHANGE_POSITION, Name = "Изменить должность", Description = "Изменить существующую должность"},
                new UserRight { Id = 12, Code = RightsRef.DELETE_POSITION, Name = "Удалить должность", Description = "Удаление должности"},

                new UserRight { Id = 13, Code = RightsRef.ALL_QUEUE_GROUPS, Name = "Группы очередей", Description = "Полный доступ к группам очередей"},
                new UserRight { Id = 14, Code = RightsRef.GET_QUEUE_GROUPS, Name = "Просмотр групп", Description = "Просмотр всех групп очередей"},
                new UserRight { Id = 15, Code = RightsRef.GET_QUEUE_GROUP, Name = "Просмотр группы", Description = "Просмотр отдельной группы очередей" },
                new UserRight { Id = 16, Code = RightsRef.CREATE_QUEUE_GROUP, Name = "Создание группы", Description = "Создание группы очередей"},
                new UserRight { Id = 17, Code = RightsRef.CHANGE_QUEUE_GROUP, Name = "Изменение группы", Description = "Изменение группы очередей"},
                new UserRight { Id = 18, Code = RightsRef.DELETE_QUEUE_GROUP, Name = "Удаление группы", Description = "Удаление группы очередей"},
                new UserRight { Id = 19, Code = RightsRef.ASSIGN_QUEUE_TO_GROUP, Name = "Привязка очереди", Description = "Привязка и отвязка очереди к группе очередей"},

                new UserRight { Id = 20, Code = RightsRef.ALL_QUEUES, Name = "Очереди", Description = "Полный доступ к очередям"},
                new UserRight { Id = 21, Code = RightsRef.GET_QUEUES, Name = "Посмотр очередей", Description = "Просмотр всех доступных очередей"},
                new UserRight { Id = 22, Code = RightsRef.GET_QUEUE, Name = "Просмотр очереди", Description = "Просмотр отдельной очереди"},
                new UserRight { Id = 23, Code = RightsRef.CREATE_QUEUE, Name = "Создание очереди", Description = "Создание очереди"},
                new UserRight { Id = 24, Code = RightsRef.CHANGE_QUEUE, Name = "Изменение очереди", Description = "Изменение очереди"},
                new UserRight { Id = 25, Code = RightsRef.DELETE_QUEUE, Name = "Удаление очереди", Description = "Удаление очереди"},
                new UserRight { Id = 26, Code = RightsRef.ASSIGN_POSITION_TO_QUEUE, Name = "Привязка должности", Description = "Привязка и отвязка должности к очереди"}
            };
        }

        public static void AfterSeed(QueueDBContext context)
        {
            List<UserRight> rights = new List<UserRight>(Seed());

            UserRole admRole = context.UserRoles.FirstOrDefault(ur => ur.Id == 1);
            UserRole manRole = context.UserRoles.FirstOrDefault(ur => ur.Id == 2);
            UserRole ticketer = context.UserRoles.FirstOrDefault(ur => ur.Id == 3);
            UserRole performer = context.UserRoles.FirstOrDefault(ur => ur.Id == 4);
            UserRole terminal = context.UserRoles.FirstOrDefault(ur => ur.Id == 5);

            admRole.UserRights.AddRange(rights);
            manRole.UserRights.AddRange(rights.Where(r => r.Id >= 1 && r.Id <= 26));
            ticketer.UserRights.AddRange(rights.Where(r => new byte[] { 14, 15, 21, 22 }.Contains(r.Id)));

            context.SaveChanges();
        }
    }
}

using System.Collections;

namespace KappaQueueCommon.Common.References
{
    public class RightsRef : IEnumerable
    {
        #region Users
        public const string ALL_USERS = "allUsers";
        public const string GET_USERS = "getUsers";
        public const string GET_USER = "getUser";
        public const string CREATE_USER = "createUser";
        public const string CHANGE_USER = "changeUser";
        public const string DELETE_USER = "deleteUser";
        #endregion
        #region QueueGroups
        public const string ALL_QUEUE_GROUPS = "allQueueGroups";
        public const string GET_QUEUE_GROUPS = "getQueueGroups";
        public const string GET_QUEUE_GROUP = "getQueueGroup";
        public const string CREATE_QUEUE_GROUP = "createQueueGroup";
        public const string CHANGE_QUEUE_GROUP = "changeQueueGroup";
        public const string DELETE_QUEUE_GROUP = "deleteQueueGroup";
        public const string ASSIGN_QUEUE_TO_GROUP = "assignQueueToGroup";
        #endregion
        #region Queues
        public const string ALL_QUEUES = "allQueues";
        public const string GET_QUEUES = "getQueues";
        public const string GET_QUEUE = "getQueue";
        public const string CREATE_QUEUE = "createQueue";
        public const string CHANGE_QUEUE = "changeQueue";
        public const string DELETE_QUEUE = "deleteQueue";
        public const string ASSIGN_POSITION_TO_QUEUE = "assignPosToQueue";
        #endregion
        #region Positions
        public const string ALL_POSITIONS = "allPositions";
        public const string GET_POSITIONS = "getPositions";
        public const string GET_POSITION = "getPosition";
        public const string CREATE_POSITION = "createPosition";
        public const string CHANGE_POSITION = "changePosition";
        public const string DELETE_POSITION = "deletePosition";
        #endregion
        #region Clients
        public const string ALL_CLIENTS = "allClients";
        public const string GET_CLIENTS = "getClients";
        public const string GET_CLIENT = "getClient";
        public const string CREATE_CLIENT = "createClient";
        public const string CHANGE_CLIENT = "changeClient";
        public const string DELETE_CLIENT = "deleteQueue";
        #endregion

        public static string Rights (params string[] param)
        {
            return string.Join(",", param);
        }

        public IEnumerator GetEnumerator()
        {
            return new string[]
            {
                ALL_USERS,
                GET_USERS,
                GET_USER,
                CREATE_USER,
                CHANGE_USER,
                DELETE_USER,
                ALL_QUEUE_GROUPS,
                GET_QUEUE_GROUPS,
                GET_QUEUE_GROUP,
                CREATE_QUEUE_GROUP,
                CHANGE_QUEUE_GROUP,
                DELETE_QUEUE_GROUP,
                ASSIGN_QUEUE_TO_GROUP,
                ALL_QUEUES,
                GET_QUEUES,
                GET_QUEUE,
                CREATE_QUEUE,
                CHANGE_QUEUE,
                DELETE_QUEUE,
                ASSIGN_POSITION_TO_QUEUE,
                ALL_POSITIONS,
                GET_POSITIONS,
                GET_POSITION,
                CREATE_POSITION,
                CHANGE_POSITION,
                DELETE_POSITION,
                ALL_CLIENTS,
                GET_CLIENTS,
                GET_CLIENT,
                CREATE_CLIENT,
                CHANGE_CLIENT,
                DELETE_CLIENT
            }.GetEnumerator();
        }
    }
}

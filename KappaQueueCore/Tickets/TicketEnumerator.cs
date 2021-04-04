using KappaQueueCore.Interfaces;
using System.Collections;

namespace KappaQueueCore.Tickets
{
    public class TicketEnumerator : ITicketEnumerator
    {
        /// <summary>
        /// Список префиксов и текущие номера для каждого префикса
        /// </summary>
        private static Hashtable _prefixes = new Hashtable();
        private object locker = new object();

        /// <summary>
        /// Очистить всю информацию по выданным талонам
        /// </summary>
        public void ClearTickets()
        {
            lock (locker)
            {
                _prefixes.Clear();
            }
        }

        /// <summary>
        /// Получить номер талона по префиксу
        /// </summary>
        /// <param name="prefix">Префикс билета</param>
        /// <returns></returns>
        public int GetTicketNumber(string prefix)
        {
            int number = 1;

            lock (locker)
            {
                if (_prefixes.ContainsKey(prefix))
                {
                    number = (int)_prefixes[prefix] + 1;
                }

                _prefixes[prefix] = number;
                return number;
            }
        }
    }
}

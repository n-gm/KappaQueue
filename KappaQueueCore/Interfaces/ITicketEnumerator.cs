namespace KappaQueueCore.Interfaces
{
    public interface ITicketEnumerator
    {
        /// <summary>
        /// Получить новый номер талона с определенным префиксом
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public int GetTicketNumber(string prefix);
        /// <summary>
        /// Очистить список талонов
        /// </summary>
        public void ClearTickets();
    }
}

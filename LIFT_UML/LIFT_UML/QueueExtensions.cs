using System.Collections.Generic;

namespace LIFT_UML
{
    /// <summary>
    /// Класс расширения для очереди
    /// </summary>
    public static class QueueExtensions
    {
        /// <summary>
        /// Получить некоторое количество элементов из очереди
        /// </summary>
        /// <param name="queue">Очередь</param>
        /// <param name="chunkSize">Колчество элементов изымаемой из очереди</param>
        /// <typeparam name="T">Тип</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> DequeueChunk<T>(this Queue<T> queue, int chunkSize) 
        {
            for (int i = 0; i < chunkSize && queue.Count > 0; i++)
            {
                yield return queue.Dequeue();
            }
        }
    }
}
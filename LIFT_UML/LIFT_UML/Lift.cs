using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LIFT_UML
{
    public class Lift
    {
        /// <summary>
        /// Вместимость лифта
        /// </summary>
        private const int LiftSize = 3;
        
        /// <summary>
        /// Очередь в лифте (можно было использовать обычный список впринципе)
        /// </summary>
        private Queue<Guest> _personQueue = new Queue<Guest>();
        
        /// <summary>
        /// Семафор для блокировки потока, когда лифт будет двигаться
        /// </summary>
        private Semaphore _semaphore;

        /// <summary>
        /// Текущее число гостей в лифте
        /// </summary>
        public int CurrentGuestCountInLift => _personQueue.Count;

        /// <summary>
        /// Свойство отвечающее за полноту лифта
        /// </summary>
        public bool IsFull => CurrentGuestCountInLift == LiftSize;

        /// <summary>
        /// Свойство отвечающее за пустоту лифта
        /// </summary>
        public bool IsEmpty => CurrentGuestCountInLift == 0;

        /// <summary>
        /// Создает экземпляр объекта <see cref="Lift"/>>
        /// </summary>
        /// <param name="semaphore"></param>
        public Lift(Semaphore semaphore)
        {
            _semaphore = semaphore;
        }

        /// <summary>
        /// Гость входит в лифт (положить гостя в лифт)
        /// </summary>
        /// <param name="guest"></param>
        public void Enqueue(Guest guest)
        {
            _personQueue.Enqueue(guest);
        }

        /// <summary>
        /// Движение лифта
        /// </summary>
        public async void Move()
        {
            // Запускаем лифт
            Console.WriteLine("Движение лифтра на 10 этаж");
            // Ждем 5 секунд
            await Task.Delay(TimeSpan.FromSeconds(5));
            Console.WriteLine("Лифт прибыл на 10 этаж");
            // Имитируем выход из лифта
            var queueLength = CurrentGuestCountInLift;
            var guests = _personQueue.DequeueChunk(LiftSize);
            Console.WriteLine($"Из лифта выходят: {String.Join(" ", guests.Select(o => o.Id))}");
            Console.WriteLine("Отправляем лифт вниз");
            // Ждем 5 секунд
            await Task.Delay(TimeSpan.FromSeconds(5));
            Console.WriteLine("Лифт внизу");
            // Освобождаем семафор
            _semaphore.Release(queueLength);
        }
        

    }
}
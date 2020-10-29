using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace LIFT_UML
{
    public class App
    {
        /// <summary>
        /// Количество гостей
        /// </summary>
        private const int GUEST_COUNT = 17;

        /// <summary>
        /// Начально количество гостей в очереди
        /// </summary>
        private const int INITIAL_GUEST_COUNT = 4;
        
        /// <summary>
        /// Генератор случайных чисел для генерации интервала прихода гостей
        /// </summary>
        private static Random _random = new Random(Environment.TickCount);
        
        /// <summary>
        /// Очередь гостей
        /// </summary>
        private static ConcurrentQueue<Guest> _guestQueue = new ConcurrentQueue<Guest>();
        
        /// <summary>
        /// Семафор для блокировки потока (чтобы гости не заходили в уехавший лифт)
        /// </summary>
        private static Semaphore _semaphore = new Semaphore(3, 3);
        
        /// <summary>
        /// Объект лифта
        /// </summary>
        private static Lift _lift = new Lift(_semaphore);
        public static void Start()
        {
            // Инициализируем начальную очередь из 4-х гостей
            for (int i = 0; i < INITIAL_GUEST_COUNT; i++)
            {
                _guestQueue.Enqueue(new Guest(Counter.Next));
            }

            // Запускаем отдельный поток для генерации прихода гостей с интервалом 6-8 секунд
            var generatedPeopleTask = Task.Run(() => GenerateGuest());
            var liftOperationTask = Task.Run(() => LiftOperation());

            // Ждем завершение работы потоков
            Task.WaitAll(generatedPeopleTask, liftOperationTask);
        }

        /// <summary>
        /// Генерация прихода гостей
        /// </summary>
        private static async void GenerateGuest()
        {
            // Будем генерировать гостей, пока не нагенерим 17
            for (int i = 5; i <= GUEST_COUNT; i++)
            {
                // Генериируем случайное число от 6 до 8 для прихода гостя
                var delay = _random.Next(6, 8);
                // Ждем delay секунд
                await Task.Delay(TimeSpan.FromSeconds(delay));
                // Помещаем пришедшего гостя в очередь
                var guest = new Guest(Counter.Next);
                _guestQueue.Enqueue(guest);
                Console.WriteLine($"Пришел новый гость: {guest.Id}");
                Console.WriteLine($"Текущая очередь из гостей: {_guestQueue.Count}");
            }

            Console.WriteLine("Гости закончились");
        }

        private static async void LiftOperation()
        {
            // Выход из условия будет осуществлен, кода исчерпаются гости и очередь из гостей будет пуста
            while (Counter.CurrentValue < GUEST_COUNT || !_guestQueue.IsEmpty || !_lift.IsEmpty)
            {
                // Если в очереди в лифт есть кто-нибудь, то выполняем
                if (!_guestQueue.IsEmpty)
                {
                    // Можем ли мы икреминтировать счетчик семафора
                    // Если да, то идем дальше
                    // Если нет, останавливаемся здесь и ждем, когда лифт освободится
                    _semaphore.WaitOne();
                    // Получаем гостя из очереди
                    var hasPersonInQueue = _guestQueue.TryDequeue(out var guest);
                    // Если смогли взять гостя из очереди
                    if (hasPersonInQueue)
                    {
                        Console.WriteLine($"Гость {guest.Id} заходит в лифт");
                        // Помещаем гостя в лифт
                        _lift.Enqueue(guest);
                        // Если лифт заполен, то отправляем его
                        if (_lift.IsFull || (Counter.CurrentValue == GUEST_COUNT && _guestQueue.IsEmpty && !_lift.IsEmpty))
                        {
                            var t = Task.Run(() => _lift.Move());
                            t.Wait(TimeSpan.FromMilliseconds(11));
                        }
                    }
                }
            }
        }
    }
}
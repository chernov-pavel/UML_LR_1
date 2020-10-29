namespace LIFT_UML
{
    /// <summary>
    /// Счетчик
    /// </summary>
    public class Counter
    {
        /// <summary>
        /// Текущее значение
        /// </summary>
        private static int _currentValue = 0;
        
        /// <summary>
        /// Свойство для получения следующего значения
        /// </summary>
        public static int Next => ++_currentValue;

        /// <summary>
        /// Свойство для получения текущего значения
        /// </summary>
        public static int CurrentValue => _currentValue;
    }
}
namespace LIFT_UML
{
    /// <summary>
    /// Гость
    /// </summary>
    public class Guest
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Создает экземпляр объекта <see cref="Guest"/>>
        /// </summary>
        /// <param name="id">Идентификатор</param>
        public Guest(int id)
        {
            Id = id;
        }
    }
}
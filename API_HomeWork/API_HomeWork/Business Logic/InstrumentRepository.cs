namespace API_HomeWork.Business_Logic
{
    public class InstrumentRepository
    {
        // Список для збереження об'єктів у пам'яті
        private readonly List<MusicalInstrument> _items = new();

        // Об'єкт-замок для синхронізації потоків
        private readonly object _lock = new();


        // 1. Отримати всі інструменти
        public List<MusicalInstrument> GetAll()
        {
            lock (_lock)
            {
                return _items.ToList();
            }
        }

        // 2. Додати новий інструмент
        public void Add(MusicalInstrument instrument)
        {
            lock (_lock)
            {
                _items.Add(instrument);
            }
        }

        // 3. Вилучити за назвою (без урахування регістру)
        public bool RemoveName(string name)
        {
            lock (_lock)
            {
                if (name is null)
                    throw new ArgumentNullException("The name can`t be null");

                //  Видаляємо всі записи, де збігається назва без урахування регістру
                int removedCount = _items.RemoveAll(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                return removedCount > 0;
            }
        }

        // 4. Вилучити всі інструменти, в яких Id > заданого
        public int RemoveWhereIdGreaterThan(int id)
        {
            lock (_lock)
            {
                return _items.RemoveAll(x => x.Id > id);
            }
        }

        // 5. Повне очищення списку
        public void Clear()
        {
            lock (_lock)
            {
                _items.Clear();
            }
        }
    }
}

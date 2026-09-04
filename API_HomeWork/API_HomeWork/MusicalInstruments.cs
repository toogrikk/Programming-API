namespace API_HomeWork;

public class MusicalInstrument
{
    private static int _globalId;

    public int Id { get; private set; }
    public string Name { get; private set; }

    private decimal _price;
    public decimal Price
    {
        get => _price;
        private set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "The price cannot be negative.");

            _price = value;
        }
    }

    public MusicalInstrument(string name, decimal price)
    {
        //  Перевіряємо щоб name не був пустий чи з пробілами/отсупами
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "Name cannot be empty.");

        Id = Interlocked.Increment(ref _globalId); //   Задаємо айді, навіть для async методів
        Name = name;
        Price = price;
    }
}
using Nullref.FullStackDemo.Database.Entity;
using Nullref.FullStackDemo.Database.Infrastructure;

namespace Nullref.FullStackDemo.Database
{
    public interface IDataContext
    {
        DbSet<Widget, Guid> Widget { get; }
    }

    public class DataContext : IDataContext
    {
        public const int MAX_ITEMS = 167;

        public DataContext()
        {
            var states = new string[] { "Georgia", "Florida", "New York", "California" };
            var enumValues = Enum.GetValues<FruitConstants>();

            //Fake Data, should be predictable, do same every time
            for (var ii = 1; ii <= MAX_ITEMS; ii++)
            {
                this.Widget.Add(new Entity.Widget
                {
                    Code = $"{ii.ToString("000")}",
                    Description = $"Some description {ii.ToString("000")}",
                    IsActive = (ii % 5 == 0),
                    MyFruit = enumValues[ii % enumValues.Length],
                    State = states[ii % states.Length],
                });
            }
        }

        public DbSet<Widget, Guid> Widget { get; private set; } = new();
    }

    public interface IDatabaseItem
    {
    }

    public interface IDatabaseItem<K> : IDatabaseItem
        where K : struct
    {
        K PrimaryKey { get; }
    }
}

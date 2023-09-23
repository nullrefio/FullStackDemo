using System.Collections;
using System.Linq.Expressions;

namespace Nullref.FullStackDemo.Database.Infrastructure
{
    /// <summary>
    /// Fake database table/collection
    /// </summary>
    public class DbSet<T, K> : IQueryable<T>
        where T : IDatabaseItem<K>
        where K : struct
    {
        private List<T> _cache = new();

        public IEnumerator<T> GetEnumerator() => _cache.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _cache.GetEnumerator();

        Type IQueryable.ElementType => typeof(T);

        Expression IQueryable.Expression => _cache.AsQueryable().Expression;

        IQueryProvider IQueryable.Provider => _cache.AsQueryable().Provider;

        public void Add(T item)
        {
            if (_cache.Any(x => x.PrimaryKey.Equals(item.PrimaryKey)))
                throw new Exception("Primary key exists.");
            _cache.Add(item);
        }

        public int Delete(K key)
            => _cache.RemoveAll(x => x.PrimaryKey.Equals(key));

        public void Update(T item)
        {
            //This is a test database so remove the old itema and insert new one
            _cache.RemoveAll(x => x.PrimaryKey.Equals(item.PrimaryKey));
            _cache.Add(item);
        }
    }
}

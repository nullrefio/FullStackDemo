using Nullref.FullStackDemo.CommonModels;

namespace Nullref.FullStackDemo.DatabaseFunctional
{
    public interface IDatabaseModelMapper<TModel, TEntity>
        where TModel : IModel
    {
        Func<TEntity, TModel> Mapper { get; }
        IPagingCriteriaModel Model { get; }
        IQueryable<TEntity> ApplyQuery(IQueryable<TEntity> source);
        IQueryable<TEntity> AsQuerable();
        bool HasAppliedSort { get; set; }
    }

    internal class MapTypeImpl<TModel, TEntity> : IDatabaseModelMapper<TModel, TEntity>
        where TModel : IModel
    {
        private IQueryable<TEntity> _source;

        public MapTypeImpl(IQueryable<TEntity> source, IPagingCriteriaModel model, Func<TEntity, TModel> mapper, Func<List<TEntity>, Task> action)
        {
            _source = source.ThrowIfArgumentNull();
            this.Model = model ?? new PagingCriteriaModel(); //can be defaulted
            this.Mapper = mapper; //might be null
        }

        public Func<TEntity, TModel> Mapper { get; private set; }
        public IPagingCriteriaModel Model { get; private set; }
        public bool HasAppliedSort { get; set; } = false;

        public IQueryable<TEntity> ApplyQuery(IQueryable<TEntity> source)
        {
            _source = source;
            return _source;
        }

        public IQueryable<TEntity> AsQuerable() => _source;
    }
}

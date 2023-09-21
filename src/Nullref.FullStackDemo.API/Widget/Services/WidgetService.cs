using Nullref.FullStackDemo.API.Widget.Models;

namespace Nullref.FullStackDemo.API.Services
{
    public interface IWidgetService
    {
        PaginatedResponseModel<WidgetModel> Get(WidgetQueryModel model);
        WidgetModel Get(Guid id);
        Guid Create(WidgetUpdateModel model);
        void Update(Guid id, WidgetUpdateModel model);
        void Delete(Guid id);
    }

    public sealed class WidgetService : IWidgetService
    {
        private readonly IDataContext _context;

        public WidgetService(IDataContext context)
        {
            _context = context.ThrowIfArgumentNull();
        }

        public PaginatedResponseModel<WidgetModel> Get(WidgetQueryModel model)
        {
            return _context.Widget
                .AsQueryable()
                .ToPagedModel(model, x => x.ToModel());
        }

        public WidgetModel Get(Guid id)
        {
            return _context.Widget
                .FirstOrDefault(x => x.Id == id)
                .ThrowIfNotFound()
                .ToModel();
        }

        public Guid Create(WidgetUpdateModel model)
        {
            var dbItem = new Database.Entity.Widget
            {
                Code = model.Code,
                Description = model.Description,
                State = model.State,
                IsActive = model.IsActive,
                MyFruit = model.MyFruit,
            };
            _context.Widget.Add(dbItem);
            return dbItem.PrimaryKey;
        }

        public void Update(Guid id, WidgetUpdateModel model)
        {
            var item = _context.Widget
                .FirstOrDefault(x => x.Id == id)
                .ThrowIfNotFound();

            item.Code = model.Code;
            item.Description = model.Description;
            item.State = model.State;
            item.IsActive = model.IsActive;
            item.MyFruit = model.MyFruit;

            _context.Widget.Update(item);
        }

        public void Delete(Guid id)
        {
            var item = _context.Widget
                .FirstOrDefault(x => x.Id == id)
                .ThrowIfNotFound();
            _context.Widget.Delete(id);
        }
    }

    file static class Extensions
    {
        public static WidgetModel ToModel(this Database.Entity.Widget item) => item == null
                ? null
                : new WidgetModel
                {
                    Code = item.Code,
                    Description = item.Description,
                    Id = item.Id,
                    IsActive = item.IsActive,
                    MyFruit = item.MyFruit,
                    State = item.State,
                };
    }
}

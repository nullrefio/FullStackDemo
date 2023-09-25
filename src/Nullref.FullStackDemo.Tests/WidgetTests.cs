using Nullref.FullStackDemo.API.Services;
using Xunit;

namespace Nullref.FullStackDemo.Tests
{
    public class WidgetTests : TestBase
    {
        [Fact]
        public void AddDatabaseItem()
        {
            Assert.NotEmpty(_context.Widget);

            var item = _context.Widget
                .OrderBy(x => x.Code)
                .Skip(10)
                .First();

            item.Code = $"{DateTime.Now.Ticks}";
            _context.Widget.Update(item);

            var item2 = _context.Widget.First(x => x.Id == item.Id);
            Assert.Equal(item.Code, item2.Code);
        }

        [Fact]
        public void AddItem()
        {
            var service = new WidgetService(_context);

            var codeWithSpaces = "   qqq    ";
            var newItem = new API.Widget.Models.WidgetModel
            {
                Code = codeWithSpaces,
                Description = "www",
                IsActive = true,
                MyFruit = Database.Entity.FruitConstants.Pear,
                State = "CO",
            };
            var id = service.Create(newItem);

            var item = _context.Widget.FirstOrDefault(x => x.Id == id);
            Assert.NotNull(item);
            Assert.Equal(newItem.Description, item.Description);
            //The 'Code' has spaces
            Assert.Equal(codeWithSpaces, newItem.Code);
            //The validation should Trim the spaces on the newly created item
            Assert.Equal(codeWithSpaces.Trim(), item.Code);
        }

        [Fact]
        public void PagingTest()
        {
            var service = new WidgetService(_context);

            var paging = new API.Widget.Models.WidgetQueryModel();
            var result = service.Get(paging);
            Assert.NotNull(result);
            Assert.Equal(Database.DataContext.MAX_ITEMS, result.TotalItems);
            Assert.Equal(10, result.Items.Count);
        }

        [Fact]
        public void PagingWithFilterTest()
        {
            var service = new WidgetService(_context);

            var paging = new API.Widget.Models.WidgetQueryModel { Search = "12" };
            var result = service.Get(paging);
            Assert.NotNull(result);
            Assert.Equal(12, result.TotalItems);
            Assert.Equal(10, result.Items.Count);

            //Select with order by clause (case insensitive)
            paging = new API.Widget.Models.WidgetQueryModel { Search = "12", Order = "CodE,AsC" };
            result = service.Get(paging);
            Assert.Equal(12, result.TotalItems);
            Assert.Equal(10, result.Items.Select(x => x.Code).Distinct().Count());
            Assert.Equal(result.Items.Select(x => x.Code).OrderBy(x => x), result.Items.Select(x => x.Code));

            //Select with order by clause (case insensitive)
            paging = new API.Widget.Models.WidgetQueryModel { Search = "12", Order = "CodE,desC" };
            result = service.Get(paging);
            Assert.Equal(12, result.TotalItems);
            Assert.Equal(10, result.Items.Select(x => x.Code).Distinct().Count());
            Assert.Equal(result.Items.Select(x => x.Code).OrderByDescending(x => x), result.Items.Select(x => x.Code));
        }

        [Fact]
        public void DeleteItem()
        {
            var service = new WidgetService(_context);
            var paging = new API.Widget.Models.WidgetQueryModel();
            var result = service.Get(paging);
            Assert.NotEmpty(result.Items);
            var count = result.TotalItems;

            service.Delete(result.Items.First().Id);
            result = service.Get(paging);
            Assert.Equal(count - 1, result.TotalItems);
        }

    }
}

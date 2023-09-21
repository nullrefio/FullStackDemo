using Nullref.FullStackDemo.API.Services;
using Microsoft.AspNetCore.Mvc;
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

            var newItem = new API.Widget.Models.WidgetModel
            {
                Code = "qqq",
                Description = "www",
                IsActive = true,
                MyFruit = Database.Entity.FruitConstants.Pear,
                State = "CO",
            };
            var id = service.Create(newItem);

            var item = _context.Widget.FirstOrDefault(x => x.Id == id);
            Assert.NotNull(item);
            Assert.Equal(newItem.Description, item.Description);
            Assert.Equal(newItem.Code, item.Code);
        }

        [Fact]
        public void PagingTest()
        {
            var service = new WidgetService(_context);

            var paging = new API.Widget.Models.WidgetQueryModel();
            var result = service.Get(paging);
            Assert.NotNull(result);
            Assert.Equal(113, result.TotalItems);
            Assert.Equal(10, result.Items.Count);
        }
    }
}

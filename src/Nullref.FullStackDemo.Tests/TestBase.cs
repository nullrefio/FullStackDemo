using Nullref.FullStackDemo.Database;

namespace Nullref.FullStackDemo.Tests
{
    public abstract class TestBase
    {
        protected IDataContext _context;

        public TestBase()
        {
            _context = new DataContext();
        }
    }
}

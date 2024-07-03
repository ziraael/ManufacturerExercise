using WarehouseService.Models;

namespace WarehouseService.Repositories
{
    public class TestQueriesRepository : ITestQueriesRepository
    {
        public Test GetById(int testID)
        {
            return new Test()
            {
                Id = 100,
                Name = "John",
            };
        }
    }
}

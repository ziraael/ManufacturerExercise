using WarehouseService.Models;
using WarehouseService.Repositories;

namespace WarehouseService.Commands
{
    public class TestCommands : ITestCommands
    {
        private readonly ITestCommandsRepository _repository;
        public TestCommands(ITestCommandsRepository repository)
        {
            _repository = repository;
        }
        public void SaveTestData(Test test)
        {
           _repository.SaveTest(test);
        }
    }
}

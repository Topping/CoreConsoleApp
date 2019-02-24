using System.Threading.Tasks;

namespace StatefulService.StatefulService
{
    public interface IBaseService
    {
        void Initialize();
        void Run();
        Task RunAsync();
    }
}
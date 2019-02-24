using System.Threading.Tasks;

namespace StatefulService.StatefulService
{
    public interface IRunnableService
    {
        Task Run();
    }
}
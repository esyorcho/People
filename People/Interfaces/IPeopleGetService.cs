using System.Threading.Tasks;

namespace People.Client.Interfaces
{
    public interface IPeopleGetService
    {
        Task RunTasks(IPrinter printer);
    }
}

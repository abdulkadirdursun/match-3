using System.Threading;
using System.Threading.Tasks;

namespace Match3.Core.Helpers
{
    public static class TaskExtensions
    {
        public static async Task WaitAsync(this Task task, CancellationToken token)
        {
            await Task.WhenAny(task, Task.Delay(Timeout.Infinite, token));
        }
    }
}
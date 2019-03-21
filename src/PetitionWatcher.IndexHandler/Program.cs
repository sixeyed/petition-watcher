using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetitionWatcher.Core;
using PetitionWatcher.MessageHandlers.IndexProspect.Indexer;
using PetitionWatcher.MessageHandlers.IndexProspect.Workers;

namespace PetitionWatcher.MessageHandlers.IndexProspect
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton(Config.Current)
                .AddSingleton<Index>()
                .AddSingleton<QueueWorker>()
                .BuildServiceProvider();

            var worker = serviceProvider.GetService<QueueWorker>();
            worker.Start();
        }
    }
}

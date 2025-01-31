using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace _Scripts.Infrastructure.WarmupSystem
{
    public class WarmupService : IWarmupService
    {
        private readonly List<IWarmupble> _services = new();
        
        public void RegisterWarmupService(IWarmupble service) => 
            _services.Add(service);

        public async UniTask Warmup()
        {
            List<UniTask> warmupTasks = Enumerable.Select(_services, service => 
                service.WarmupAsync()).ToList();

            await UniTask.WhenAll(warmupTasks);
        }
    }

    public interface IWarmupService
    {
        UniTask Warmup();
        void RegisterWarmupService(IWarmupble service);
    }
    
    public interface IWarmupble
    {
        UniTask WarmupAsync();
    }
}
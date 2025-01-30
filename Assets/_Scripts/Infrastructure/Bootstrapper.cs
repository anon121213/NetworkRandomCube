using _Scripts.Infrastructure.WarmupSystem;
using VContainer.Unity;

namespace _Scripts.Infrastructure
{
    public class Bootstrapper : IInitializable
    {
        private readonly IWarmupService _warmupService;

        public Bootstrapper(IWarmupService WarmupService)
        {
            _warmupService = WarmupService;
        }

        public async void Initialize()
        {
            await _warmupService.Warmup();
        }
    }
}    
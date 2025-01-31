using _Scripts.Gameplay.CubeComponent;
using _Scripts.Gameplay.CubeRoller;
using _Scripts.Gameplay.CubeSpawner;
using _Scripts.Gameplay.RollQueue;
using _Scripts.Gameplay.WinSystem;
using _Scripts.Infrastructure.AddressableLoader;
using _Scripts.Infrastructure.Factory;
using _Scripts.Infrastructure.LobbySystem;
using _Scripts.Infrastructure.SceneLoader;
using _Scripts.Infrastructure.StaticData.Data;
using _Scripts.Infrastructure.StaticData.Provider;
using _Scripts.Infrastructure.WarmupSystem;
using _Scripts.Netcore.FormatterSystem;
using _Scripts.Netcore.Initializer;
using _Scripts.Netcore.RPCSystem;
using _Scripts.Netcore.RPCSystem.Callers;
using _Scripts.Netcore.RPCSystem.DynamicProcessor;
using _Scripts.Netcore.RPCSystem.Processors;
using _Scripts.Netcore.Runner;
using _Scripts.Netcore.Spawner;
using _Scripts.Netcore.Spawner.ObjectsSyncer;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Installers
{
    public class MainInstaller : LifetimeScope
    {
        [SerializeField] private AllData _allData;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<Bootstrapper>();
            
            builder.Register<INetworkRunner, NetworkRunner>(Lifetime.Singleton);
            builder.Register<INetworkFormatter, NetworkFormatter>(Lifetime.Singleton);
            builder.Register<IRpcListener, RPCListener>(Lifetime.Singleton);
            builder.Register<ICallerService, CallerService>(Lifetime.Singleton);
            builder.Register<IRpcReceiveProcessor, RpcReceiveReceiveProcessor>(Lifetime.Singleton);
            builder.Register<IRPCSendProcessor, RPCSendProcessor>(Lifetime.Singleton);
            builder.Register<IDynamicProcessorService, DynamicProcessorService>(Lifetime.Singleton);
            builder.Register<INetworkInitializer, NetworkInitializer>(Lifetime.Singleton);
            builder.Register<INetworkObjectSyncer, NetworkObjectsSyncer>(Lifetime.Singleton);
            builder.Register<INetworkSpawner, NetworkSpawner>(Lifetime.Singleton);
            
            builder.Register<ILobbyManager, LobbiesHandler>(Lifetime.Singleton);
            builder.Register<IStaticDataProvider, StaticDataProvider>(Lifetime.Singleton).WithParameter(_allData);
            builder.Register<ILobbyPanelFactory, LobbyPanelFactory>(Lifetime.Singleton);
            builder.Register<IAssetProvider, AssetProvider>(Lifetime.Singleton);
            builder.Register<IWarmupService, WarmupService>(Lifetime.Singleton);
            builder.Register<ICubeSpawner, CubeSpawner>(Lifetime.Singleton);
            builder.Register<ISceneLoader, SceneLoader.SceneLoader>(Lifetime.Singleton);
            builder.Register<ICubeRoller, CubeRoller>(Lifetime.Singleton);
            builder.Register<ICubeRollerChecker, CubeRollerChecker>(Lifetime.Singleton);
            builder.Register<IQueueService, QueueService>(Lifetime.Singleton);
            builder.Register<IWinService, WinService>(Lifetime.Singleton);
        }
    }
}
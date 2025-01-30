using _Scripts.Infrastructure.AddressableLoader;
using _Scripts.Infrastructure.ConnectWindow;
using _Scripts.Infrastructure.Factory;
using _Scripts.Infrastructure.LobbySystem;
using _Scripts.Infrastructure.StaticData;
using _Scripts.Netcore.Data.NetworkObjects;
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
        [SerializeField] private NetworkObjectsConfig _networkObjectsConfig;
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
            builder.Register<INetworkSpawner, NetworkSpawner>(Lifetime.Singleton).WithParameter(_networkObjectsConfig);
            
            builder.Register<ILobbyManager, LobbiesHandler>(Lifetime.Singleton);
            builder.Register<IStaticDataProvider, StaticDataProvider>(Lifetime.Singleton).WithParameter(_allData);
            builder.Register<ILobbyPanelFactory, LobbyPanelFactory>(Lifetime.Singleton);
            builder.Register<IAssetProvider, AssetProvider>(Lifetime.Singleton);
        }
    }
}
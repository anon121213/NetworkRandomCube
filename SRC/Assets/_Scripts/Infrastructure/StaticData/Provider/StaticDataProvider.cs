using _Scripts.Infrastructure.StaticData.Data;
using _Scripts.Netcore.Data.NetworkObjects;

namespace _Scripts.Infrastructure.StaticData.Provider
{
    public class StaticDataProvider : IStaticDataProvider
    {
        public StaticDataProvider(AllData allData)
        {
            AssetsReferences = allData.AssetsReferences;
            CubeDiceSettings = allData.CubeDiceSettings;
            CubeRollerSettings = allData.CubeRollerSettings;
            NetworkObjectsConfig = allData.NetworkObjectsConfig;
        }

        public AssetsReferences AssetsReferences { get; }
        public CubeDiceSettings CubeDiceSettings { get; }
        public CubeRollerSettings CubeRollerSettings { get; }
        public NetworkObjectsConfig NetworkObjectsConfig { get; }
    }
}
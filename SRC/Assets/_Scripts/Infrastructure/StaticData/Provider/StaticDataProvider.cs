using _Scripts.Infrastructure.StaticData.Data;

namespace _Scripts.Infrastructure.StaticData.Provider
{
    public class StaticDataProvider : IStaticDataProvider
    {
        public StaticDataProvider(AllData allData)
        {
            AssetsReferences = allData.AssetsReferences;
            CubeDiceSettings = allData.CubeDiceSettings;
            CubeRollerSettings = allData.CubeRollerSettings;
        }

        public AssetsReferences AssetsReferences { get; }
        public CubeDiceSettings CubeDiceSettings { get; }
        public CubeRollerSettings CubeRollerSettings { get; }
    }
}
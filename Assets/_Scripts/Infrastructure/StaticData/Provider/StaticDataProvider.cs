using _Scripts.Infrastructure.StaticData.Data;

namespace _Scripts.Infrastructure.StaticData.Provider
{
    public class StaticDataProvider : IStaticDataProvider
    {
        public StaticDataProvider(AllData allData)
        {
            AssetsReferences = allData.AssetsReferences;
        }

        public AssetsReferences AssetsReferences { get; }
    }
}
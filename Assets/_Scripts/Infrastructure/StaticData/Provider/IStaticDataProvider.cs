using _Scripts.Infrastructure.StaticData.Data;

namespace _Scripts.Infrastructure.StaticData.Provider
{
    public interface IStaticDataProvider
    {
        AssetsReferences AssetsReferences { get; }
        CubeDiceSettings CubeDiceSettings { get; }
    }
}
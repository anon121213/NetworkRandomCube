using _Scripts.Infrastructure.StaticData.Data;
using _Scripts.Netcore.Data.NetworkObjects;

namespace _Scripts.Infrastructure.StaticData.Provider
{
    public interface IStaticDataProvider
    {
        AssetsReferences AssetsReferences { get; }
        CubeDiceSettings CubeDiceSettings { get; }
        CubeRollerSettings CubeRollerSettings { get; }
        NetworkObjectsConfig NetworkObjectsConfig { get; }
    }
}
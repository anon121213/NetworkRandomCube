using MessagePack;
using MessagePack.Formatters;
using UnityEngine.AddressableAssets;

namespace _Scripts.Netcore.FormatterSystem.Formatters
{
    public class AssetReferenceGameObjectFormatter : IMessagePackFormatter<AssetReferenceGameObject>
    {
        public void Serialize(ref MessagePackWriter writer, AssetReferenceGameObject value, MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }
        
            writer.Write(value.AssetGUID);
        }

        public AssetReferenceGameObject Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            string assetGUID = reader.ReadString();
            return new AssetReferenceGameObject(assetGUID);
        }
    }

}
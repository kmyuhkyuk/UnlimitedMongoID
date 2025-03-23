using BepInEx;
using SPT.Common.Http;
using UnlimitedMongoID.Attributes;
using UnlimitedMongoID.Patches;

namespace UnlimitedMongoID
{
    [BepInPlugin("com.kmyuhkyuk.UnlimitedMongoID", "UnlimitedMongoID", "1.1.3")]
    [EFTConfigurationPluginAttributes("https://github.com/kmyuhkyuk/UnlimitedMongoID")]
    public class UnlimitedMongoIDPlugin : BaseUnityPlugin
    {
        private void Start()
        {
            new MongoIDConstructorPatch().Enable();
            new ConvertTimeStampPatch().Enable();
            new ConvertCounterPatch().Enable();

            RequestHandler.PutJson("/unlimited-mongoid/client", string.Empty);
        }
    }
}
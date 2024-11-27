using BepInEx;
using SPT.Common.Http;
using UnlimitedMongoID.Attributes;
using UnlimitedMongoID.Patches;

namespace UnlimitedMongoID
{
    [BepInPlugin("com.kmyuhkyuk.UnlimitedMongoID", "UnlimitedMongoID", "1.0.0")]
    [EFTConfigurationPluginAttributes("https://hub.sp-tarkov.com/files/file/2466-unlimited-mongoid")]
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
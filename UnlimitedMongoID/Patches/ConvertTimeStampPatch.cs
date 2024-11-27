using System.Globalization;
using System.Linq;
using System.Reflection;
using EFT;
using SPT.Reflection.Patching;

namespace UnlimitedMongoID.Patches
{
    public class ConvertTimeStampPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(MongoID).GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public)
                .Single(x => x.ReturnType == typeof(uint) && x.GetParameters().Length == 1);
        }

        [PatchPrefix]
        private static bool Prefix(string id, ref uint __result)
        {
            if (TryGetTimeStampHashCode(id, out var hashCode))
            {
                __result = hashCode;

                return false;
            }

            return true;
        }

        private static bool TryGetTimeStampHashCode(string id, out uint hashCode)
        {
            if (id.Length != 24 || !uint.TryParse(id.Substring(0, 8), NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture, out _))
            {
                hashCode = (uint)id.GetHashCode();

                return true;
            }

            hashCode = 0;

            return false;
        }
    }
}
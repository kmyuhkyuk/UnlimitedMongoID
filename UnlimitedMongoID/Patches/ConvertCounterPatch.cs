using System.Linq;
using System.Reflection;
using EFT;
using SPT.Reflection.Patching;

namespace UnlimitedMongoID.Patches
{
    public class ConvertCounterPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(MongoID).GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public)
                .Single(x => x.ReturnType == typeof(ulong) && x.GetParameters().Length == 1);
        }

        [PatchPrefix]
        private static bool Prefix(string id, ref ulong __result)
        {
            if (TryGetCounterHashCode(id, out var hashCode))
            {
                __result = hashCode;

                return false;
            }

            return true;
        }

        private static bool TryGetCounterHashCode(string id, out ulong hashCode)
        {
            if (id.Length != 24 || !ulong.TryParse(id, out _))
            {
                hashCode = (ulong)id.GetHashCode();

                return true;
            }

            hashCode = 0;

            return false;
        }
    }
}
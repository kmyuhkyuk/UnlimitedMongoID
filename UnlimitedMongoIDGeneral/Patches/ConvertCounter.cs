namespace UnlimitedMongoIDGeneral
{
    public partial class UnlimitedMongoIDGeneralPlugin
    {
        private static bool ConvertCounter(string id, ref ulong __result)
        {
            if (TryGetCounterHashCode(id, out var hashCode))
            {
                __result = hashCode;

                return false;
            }

            return true;
        }
    }
}
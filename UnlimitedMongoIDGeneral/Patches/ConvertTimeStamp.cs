namespace UnlimitedMongoIDGeneral
{
    public partial class UnlimitedMongoIDGeneralPlugin
    {
        private static bool ConvertTimeStamp(string id, ref uint __result)
        {
            if (TryGetTimeStampHashCode(id, out var hashCode))
            {
                __result = hashCode;

                return false;
            }

            return true;
        }
    }
}
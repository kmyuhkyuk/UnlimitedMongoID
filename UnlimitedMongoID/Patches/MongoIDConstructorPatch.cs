using System.Linq;
using System.Reflection;
using EFT;
using MonoMod.Cil;
using SPT.Reflection.Patching;

namespace UnlimitedMongoID.Patches
{
    public class MongoIDConstructorPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(MongoID).GetConstructors().Single(x => x.GetParameters()[0].ParameterType == typeof(string));
        }

        [PatchILManipulator]
        private static void ILManipulator(ILContext il)
        {
            var codes = il.Instrs;

            var cursor = new ILCursor(il);

            var brTrueIndex = cursor.GotoNext(x => x.MatchBrtrue(out _)).Index;

            var brFalseIndex = cursor.GotoNext(x => x.MatchBrfalse(out _)).Index;

            //Skip id.Length != 24
            codes[brTrueIndex].Operand = codes[brFalseIndex].Operand;
        }
    }
}
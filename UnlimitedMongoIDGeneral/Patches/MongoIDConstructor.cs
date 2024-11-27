using System;
using EFTApi;
using HarmonyLib;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace UnlimitedMongoIDGeneral
{
    public partial class UnlimitedMongoIDGeneralPlugin
    {
        private static void MongoIDConstructor(ILContext il)
        {
            var codes = il.Instrs;

            var cursor = new ILCursor(il);

            if (EFTVersion.AkiVersion > EFTVersion.Parse("2.3.1") && EFTVersion.AkiVersion < EFTVersion.Parse("3.6.0"))
            {
                var callToUInt32Index = cursor.GotoNext(x =>
                    x.MatchCall(AccessTools.Method(typeof(Convert), nameof(Convert.ToUInt32),
                        new[] { typeof(string), typeof(int) }))).Index;

                var callToUInt64Index = cursor.GotoNext(x =>
                    x.MatchCall(AccessTools.Method(typeof(Convert), nameof(Convert.ToUInt64),
                        new[] { typeof(string), typeof(int) }))).Index;

                //Nop String.Substring
                for (var i = callToUInt32Index - 4; i < callToUInt32Index; i++)
                {
                    codes[i].OpCode = OpCodes.Nop;
                }

                for (var i = callToUInt64Index - 4; i < callToUInt64Index; i++)
                {
                    codes[i].OpCode = OpCodes.Nop;
                }

                codes[callToUInt32Index].Operand =
                    AccessTools.Method(typeof(UnlimitedMongoIDGeneralPlugin), nameof(GetTimeStamp));

                codes[callToUInt64Index].Operand =
                    AccessTools.Method(typeof(UnlimitedMongoIDGeneralPlugin), nameof(GetCounter));
            }
            else if (EFTVersion.AkiVersion > EFTVersion.Parse("3.9.8"))
            {
                var brTrueIndex = cursor.GotoNext(x => x.MatchBrtrue(out _)).Index;

                var brFalseIndex = cursor.GotoNext(x => x.MatchBrfalse(out _)).Index;

                //Skip id.Length != 24
                codes[brTrueIndex].Operand = codes[brFalseIndex].Operand;
            }
        }
    }
}
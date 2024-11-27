using System;
using System.Linq;
using EFT;
using EFTApi;
using EFTReflection;
using JetBrains.Annotations;

namespace UnlimitedMongoIDGeneral.Models
{
    internal class ReflectionModel
    {
        private static readonly Lazy<ReflectionModel> Lazy = new Lazy<ReflectionModel>(() => new ReflectionModel());

        public static ReflectionModel Instance => Lazy.Value;

        public readonly RefHelper.HookRef MongoIDConstructor;

        [CanBeNull] public readonly RefHelper.HookRef ConvertTimeStamp;

        [CanBeNull] public readonly RefHelper.HookRef ConvertCounter;

        private ReflectionModel()
        {
            var mongoIDType = typeof(MongoID);

            MongoIDConstructor = RefHelper.HookRef.Create(mongoIDType.GetConstructors()
                .Single(x => x.GetParameters()[0].ParameterType == typeof(string)));

            if (EFTVersion.AkiVersion > EFTVersion.Parse("3.5.8"))
            {
                ConvertTimeStamp = RefHelper.HookRef.Create(mongoIDType,
                    x => x.ReturnType == typeof(uint) && x.GetParameters().Length == 1);

                ConvertCounter = RefHelper.HookRef.Create(mongoIDType,
                    x => x.ReturnType == typeof(ulong) && x.GetParameters().Length == 1);
            }
        }
    }
}
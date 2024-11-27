﻿using System;
using BepInEx;
using HarmonyLib;
using UnlimitedMongoIDGeneral.Attributes;
using UnlimitedMongoIDGeneral.Models;
using static EFTApi.EFTHelpers;

namespace UnlimitedMongoIDGeneral
{
    [BepInPlugin("com.kmyuhkyuk.UnlimitedMongoIDGeneral", "UnlimitedMongoIDGeneral", "1.0.0")]
    [BepInDependency("com.kmyuhkyuk.EFTApi", "1.2.2")]
    [EFTConfigurationPluginAttributes("https://hub.sp-tarkov.com/files/file/2466-unlimited-mongoid")]
    public partial class UnlimitedMongoIDGeneralPlugin : BaseUnityPlugin
    {
        private void Start()
        {
            var reflectionModel = ReflectionModel.Instance;

            reflectionModel.MongoIDConstructor.Add(this, nameof(MongoIDConstructor), HarmonyPatchType.ILManipulator);
            reflectionModel.ConvertTimeStamp?.Add(this, nameof(ConvertTimeStamp), HarmonyPatchType.Prefix);
            reflectionModel.ConvertCounter?.Add(this, nameof(ConvertCounter), HarmonyPatchType.Prefix);

            _RequestHandlerHelper.PutJson("/unlimited-mongoid/client", string.Empty);
        }

        private static uint GetTimeStamp(string id)
        {
            return !TryGetTimeStampHashCode(id, out var hashCode) ? Convert.ToUInt32(id.Substring(0, 8), 16) : hashCode;
        }

        private static ulong GetCounter(string id)
        {
            return !TryGetCounterHashCode(id, out var hashCode) ? Convert.ToUInt64(id.Substring(8, 16), 16) : hashCode;
        }

        private static bool TryGetTimeStampHashCode(string id, out uint hashCode)
        {
            if (id.Length != 24 || !uint.TryParse(id, out _))
            {
                hashCode = (uint)id.GetHashCode();

                return true;
            }

            hashCode = 0;

            return false;
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
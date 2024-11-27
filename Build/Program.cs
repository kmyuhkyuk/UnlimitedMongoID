using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CopyBuildAssembly;

// ReSharper disable ClassNeverInstantiated.Global

namespace Build
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var arg = args.ElementAtOrDefault(0);
            //var sha = Copy.GetTipSha(args.ElementAtOrDefault(1));

            const string gamePath = @"R:\Battlestate Games\Client.0.15.5.1.33420";

            var modPath = $@"{gamePath}\BepInEx\plugins\kmyuhkyuk-UnlimitedMongoID";

            const string versionName = "1.0.0";

            var releaseName = $"{new DirectoryInfo(modPath).Name}(Release_{versionName}).7z";

            try
            {
                var unlimitedMongoIDServerModPath = Path.Combine(baseDirectory, "UnlimitedMongoID_ServerMod");

                Copy.CopyFolder(arg, "Release", Path.Combine(unlimitedMongoIDServerModPath), gamePath);

                Copy.CopyAssembly(arg, "Release", baseDirectory, modPath, new[]
                {
                    "UnlimitedMongoID"
                } /*, sha*/);

                Copy.GenerateSevenZip(arg, "Release", modPath, releaseName, @"BepInEx\plugins", Array.Empty<string>(),
                    Array.Empty<string>(), Array.Empty<string>(),
                    new[] { unlimitedMongoIDServerModPath });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                Console.ReadKey();

                Process.GetCurrentProcess().Kill();
            }
        }
    }
}
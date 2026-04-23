using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;
//using CheatsRoma.Behaviors;
using CheatsRoma.Cheats;

namespace CheatsRoma
{
    public class SubModule : MBSubModuleBase
    {
        private const string HarmonyId = "com.cheatsroma.mod";
        private Harmony _harmony;
        private bool _patchesApplied = false;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            _harmony = new Harmony(HarmonyId);
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            InformationManager.DisplayMessage(new InformationMessage("CheatsRoma loaded successfully!", Colors.Green));
        }

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);

            // Здесь можно добавлять MissionBehavior (например, бесконечные патроны через Behavior)
            // mission.AddMissionBehavior(new InfiniteAmmoBehavior());
        }

        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);

            if (_patchesApplied)
                return;

            // Применяем все Harmony патчи, найденные в сборке
            PATCH(_harmony, ref _patchesApplied);

            if (_patchesApplied)
            {
                InformationManager.DisplayMessage(new InformationMessage("CheatsRoma: All patches applied!", Colors.Green));
            }
        }

        /// <summary>
        /// Применяет все Harmony патчи, определённые в текущей сборке.
        /// </summary>
        internal static void PATCH(Harmony patcher, ref bool patchesApplied)
        {
            Assembly assembly = typeof(SubModule).Assembly;
            Type[] typesFromAssembly = AccessTools.GetTypesFromAssembly(assembly);
            List<string> failedPatches = new List<string>();

            foreach (Type type in typesFromAssembly)
            {
                try
                {
                    // Проверяем, есть ли у класса атрибут HarmonyPatch
                    if (type.GetCustomAttributes(typeof(HarmonyPatch), false).Any())
                    {
                        new PatchClassProcessor(patcher, type).Patch();
                    }
                }
                catch (HarmonyException ex)
                {
                    failedPatches.Add($"{type.Name}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    failedPatches.Add($"{type.Name}: {ex.Message}");
                }
            }

            patchesApplied = true;

            if (failedPatches.Any())
            {
                string errorList = string.Join(Environment.NewLine, failedPatches);
                InformationManager.ShowInquiry(new InquiryData(
                    "CheatsRoma - Patch Failed",
                    $"Some patches could not be applied:{Environment.NewLine}{errorList}",
                    true,
                    false,
                    "OK",
                    null,
                    null,
                    null));
            }
        }

        /// <summary>
        /// Логирует ошибку в файл и показывает сообщение пользователю (опционально).
        /// </summary>
        internal static void LogError(Exception e, Type type = null)
        {
            try
            {
                string errorFilePath = CreateErrorFile(e, type);
                InformationManager.DisplayMessage(new InformationMessage($"CheatsRoma error logged: {errorFilePath}", Colors.Red));
            }
            catch
            {
                // Игнорируем ошибки логирования
            }
        }

        private static string CreateErrorFile(Exception e, Type type = null)
        {
            string fileName = $"CheatsRoma_Error_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt";
            string assemblyLocation = Assembly.GetAssembly(typeof(SubModule)).Location;
            string directoryName = Path.GetDirectoryName(assemblyLocation);
            string filePath = Path.Combine(directoryName, fileName);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CheatsRoma Error Report");
            sb.AppendLine($"Time: {DateTime.Now}");
            sb.AppendLine();
            sb.AppendLine("Loaded Modules:");
            foreach (ModuleInfo module in ModuleHelper.GetModules())
            {
                sb.AppendLine($"  {module.Name} {module.Version}");
            }
            sb.AppendLine();

            if (type != null)
            {
                var patchAttr = type.GetCustomAttribute<HarmonyPatch>();
                sb.AppendLine("Failed Patch:");
                sb.AppendLine($"  Type: {type.FullName}");
                if (patchAttr?.info.declaringType != null)
                    sb.AppendLine($"  Target: {patchAttr.info.declaringType.FullName}.{patchAttr.info.methodName}");
                sb.AppendLine();
            }

            sb.AppendLine("Exception:");
            sb.AppendLine(e.ToString());

            File.WriteAllText(filePath, sb.ToString());
            return filePath;
        }
    }
}
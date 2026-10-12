using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using GraveyardKeeper2LocalizationAPI.Api;
using HarmonyLib;

namespace FreeConveyors
{
    [BepInPlugin(Guid, "Free Conveyors", "1.4.0")]
    [BepInDependency("IceBoxStudio.GraveyardKeeper2.LocalizationAPI")]
    public class Plugin : BaseUnityPlugin
    {
        public const string Guid = "enoc.gk2.freeconveyors";

        // Los nombres de seccion/clave del .cfg son siempre en ingles (identificadores estables);
        // lo que ve el jugador en el menu sale de FreeConveyors.Localization.json segun el idioma del juego.
        private const string SectionBelts = "1. Belts";
        private const string SectionMachines = "2. Machines";
        private const string SectionDebug = "3. Debug";

        // Maquinas conocidas (ids vistos en el juego). Las desconocidas se registran solas al aparecer.
        private static readonly string[] KnownMachines =
        {
            "conveyor_assemblybench_t1", "conveyor_assemblybench_t2",
            "conveyor_furnace_t1",
            "conveyor_kitchen_t1", "conveyor_kitchen_t2",
            "conveyor_bioreactor",
            "conveyor_woodworkbench",
        };

        internal static BepInEx.Logging.ManualLogSource Log;
        internal static ConfigFile Cfg;
        internal static ModLocalizer L;
        internal static ConfigEntry<bool> DebugLog;
        private static readonly Dictionary<ConveyorElementType, ConfigEntry<bool>> Free = new Dictionary<ConveyorElementType, ConfigEntry<bool>>();
        private static readonly Dictionary<string, ConfigEntry<int>> Machine = new Dictionary<string, ConfigEntry<int>>();

        private void Awake()
        {
            Log = Logger;
            Cfg = Config;

            L = LocalizationApi.For(Guid);
            L.RegisterJson(Path.Combine(Path.GetDirectoryName(Info.Location), "FreeConveyors.Localization.json"));

            // true = NO consume engranajes. Por defecto solo las cintas son gratis.
            int order = 100;
            BindFree(ConveyorElementType.Cell, true, order--);
            BindFree(ConveyorElementType.UndergroundCell, true, order--);
            BindFree(ConveyorElementType.StationCell, true, order--);
            BindFree(ConveyorElementType.Splitter, true, order--);
            BindFree(ConveyorElementType.Chest, false, order--);
            BindFree(ConveyorElementType.ChestOut, false, order--);
            BindFree(ConveyorElementType.Pallet, false, order--);

            foreach (var id in KnownMachines) GetMachineEntry(id);

            DebugLog = Config.Bind(SectionDebug, "LogConsumption", false,
                L.Config("debug.log", 100, SectionDebug, "section.debug", 10));

            new Harmony(Guid).PatchAll();
            Logger.LogInfo("Free Conveyors loaded.");
        }

        private void BindFree(ConveyorElementType type, bool def, int order)
        {
            Free[type] = Config.Bind(SectionBelts, type.ToString(), def,
                L.Config("belts." + type.ToString().ToLowerInvariant(), order, SectionBelts, "section.belts", 100));
        }

        private static ConfigEntry<int> GetMachineEntry(string id)
        {
            if (!Machine.TryGetValue(id, out var entry))
            {
                // Nivel 1 (_t1) = 3, el resto (nivel 2, biorreactor, maquinas nuevas) = 5.
                int def = id.EndsWith("_t1") ? 3 : 5;
                entry = Cfg.Bind(SectionMachines, id, def,
                    L.Config("machine." + id, 100 - Machine.Count, SectionMachines, "section.machines", 50,
                        new AcceptableValueRange<int>(0, 100)));
                Machine[id] = entry;
            }
            return entry;
        }

        internal static int MachineCost(string id) => GetMachineEntry(id).Value;

        internal static bool IsFree(ConveyorElementType type)
        {
            return Free.TryGetValue(type, out var e) && e.Value;
        }
    }

    [HarmonyPatch(typeof(ConveyorWgoData), "GetQuality")]
    internal static class GetQualityPatch
    {
        private static readonly HashSet<string> Seen = new HashSet<string>();

        // Quality negativa = consumo de engranajes (las PowerSource son positivas y no se tocan).
        private static void Postfix(ConveyorWgoData __instance, ref float __result)
        {
            var def = __instance.Definition;
            if (def == null) return;

            if (Plugin.DebugLog.Value && Seen.Add(def.id + "|" + __result))
                Plugin.Log.LogInfo($"[CONSUMPTION] type={def.conveyorType} id={def.id} original={__result}");

            if (def.conveyorType == ConveyorElementType.Workbench)
            {
                // Vanilla: las maquinas valen 0. Les asignamos un consumo configurable por id.
                if (__result == 0f)
                    __result = -Plugin.MachineCost(def.id);
            }
            else if (__result < 0f && Plugin.IsFree(def.conveyorType))
            {
                __result = 0f;
            }
        }
    }
}

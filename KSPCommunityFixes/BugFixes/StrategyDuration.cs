﻿using System;
using System.Collections.Generic;
using HarmonyLib;
using Strategies;
using System.Reflection.Emit;

namespace KSPCommunityFixes.BugFixes
{
    class StrategyDuration : BasePatch
    {
        protected override Version VersionMin => new Version(1, 8, 0);

        protected override void ApplyPatches()
        {
            AddPatch(PatchType.Prefix,
                AccessTools.PropertyGetter(typeof(Strategy), nameof(Strategy.LongestDuration)),
                nameof(Strategy_LongestDuration));

            AddPatch(PatchType.Prefix,
                AccessTools.PropertyGetter(typeof(Strategy), nameof(Strategy.LeastDuration)),
                nameof(Strategy_LeastDuration));

            AddPatch(PatchType.Transpiler, typeof(Strategy), nameof(Strategy.CanBeDeactivated));

            AddPatch(PatchType.Transpiler, typeof(Strategy), nameof(Strategy.SendStateMessage));
        }

        static bool Strategy_LongestDuration(Strategy __instance, ref double __result)
        {
            __result = __instance.FactorLerp(__instance.MinLongestDuration, __instance.MaxLongestDuration);
            return false;
        }

        static bool Strategy_LeastDuration(Strategy __instance, ref double __result)
        {
            __result = __instance.FactorLerp(__instance.MinLeastDuration, __instance.MaxLeastDuration);
            return false;
        }

        static IEnumerable<CodeInstruction> Strategy_CanBeDeactivated_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> code = new List<CodeInstruction>(instructions);
            for (int i = 9; i < code.Count; ++i)
            {
                // We need to fix the inequality check for if ( dateActivated + LeastDuration < Planetarium.fetch.time )
                // because that should be a > check.
                if (code[i].opcode == OpCodes.Bge_Un_S)
                {
                    code[i].opcode = OpCodes.Ble_Un_S;
                    break;
                }
            }

            return code;
        }

        // Fix a Squad typo where </> was used instead of </b>
        static IEnumerable<CodeInstruction> Strategy_SendStateMessage_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> code = new List<CodeInstruction>(instructions);
            for (int i = 0; i < code.Count; ++i)
            {
                if (code[i].opcode == OpCodes.Ldstr && (code[i].operand as string) == "</>\n\n")
                {
                    code[i].operand = "</b>\n\n";
                    break;
                }
            }

            return code;
        }
    }
}

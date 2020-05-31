using System.Collections.Generic;
using Harmony;
using HBS.DebugConsole;
using UnityEngine;

namespace MechEngineer.Features.Globals.Patches
{
    [HarmonyPatch(typeof(DebugConsoleHelper), "LateUpdate")]
    public static class DebugConsoleHelper_LastUpdate_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions
                .MethodReplacer(
                    AccessTools.Method(typeof(Input), nameof(Input.GetKeyUp), new[] { typeof(KeyCode) }),
                    AccessTools.Method(typeof(DebugConsoleHelper_LastUpdate_Patch), nameof(GetKeyUp))
                );
        }

        public static bool GetKeyUp(KeyCode code)
        {
            if (code == KeyCode.BackQuote)
            {
                return Input.GetKeyUp(KeyCode.Backspace);
            }
            return Input.GetKeyUp(code);
        }
    }
}
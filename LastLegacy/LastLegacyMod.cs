using HarmonyLib;
using UnityEngine;
using STRINGS;

namespace LastLegacy
{
    [HarmonyPatch(typeof(MinionModifiers), "OnDeath")]
    public class LastLegacyMod : KMod.UserMod2
    {

        [HarmonyPostfix]
        public static void OnDeathPost(MinionModifiers __instance)
        {
            int[] pos = getDropSpawnLocation(__instance.gameObject);
            foreach (int p in pos)
            {
                GameObject gameObject = Scenario.SpawnPrefab(p, 0, 0, "Meat", Grid.SceneLayer.Ore);
                gameObject.SetActive(true);
                Edible component2 = gameObject.GetComponent<Edible>();
                if (component2 != null)
                {
                    ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component2.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.BUTCHERED, "{0}", gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.BUTCHERED_CONTEXT);
                }
            }
        }

        private static int[] getDropSpawnLocation(GameObject obj)
        {
            int num = Grid.PosToCell(obj);
            int idx = 0;
            for (int x = -1; x <= 1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
                {
                    _dropPosition[idx] = Grid.OffsetCell(num, x, y);
                    ++idx;
                }
            }
            for (int ix = 0; ix < _dropPosition.Length; ++ix)
                _dropPosition[ix] = getValidCell(_dropPosition[ix]);
            return _dropPosition;
        }

        private static int getValidCell(int pos)
        {
            if (Grid.IsValidCell(pos) && !Grid.Solid[pos])
                return pos;
            for (int x = -1; x <= 1; ++x)   //left first
            {
                for (int y = 1; y >= -1; ++y)   //above first
                {
                    CellOffset offset;
                    offset.x = x;
                    offset.y = y;
                    int np = Grid.OffsetCell(pos, offset);
                    if (Grid.IsValidCell(np) && !Grid.Solid[np])
                        return np;
                }
            }
            return pos;
        }

        private static int[] _dropPosition = new int[9];
    }
}

using CMP.Scripts.Helper;
using UnityEditor;
using UnityEngine;

namespace CMP.Scripts
{
    public static class GhostGizmoDrawer
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void DrawGhostGizmos(Ghost ghost, GizmoType type)
        {
            if (!Application.isPlaying)
                return;

            DrawSightRay(ghost);
            DrawStateLabel(ghost);
        }

        private static void DrawSightRay(Ghost ghost)
        {
            Vector2Int dir = ghost.Heading.ToVector2Int();
            if (dir == Vector2Int.zero)
                return;

            GridData grid = AssetDatabase.Instance.GridData;
            Vector2Int cell = ghost.CurrentGridPos;

            for (int i = 0; i < GameSettings.AiSightRange; i++)
            {
                Vector2Int next = cell + dir;
                if (grid.GetCellAtOrDefault(next, CellType.Wall) == CellType.Wall)
                    break;
                cell = next;
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(ghost.transform.position, cell.ToWorld());
        }

        private static void DrawStateLabel(Ghost ghost)
        {
            var style = new GUIStyle { fontSize = 14, normal = { textColor = Color.white } };
            Handles.Label(ghost.transform.position + Vector3.up * 0.4f, ghost.State.ToString(), style);
        }
    }
}
using CMP.Scripts;
using CMP.Scripts.Helper;
using UnityEditor;
using UnityEngine;

namespace CMP.Editor
{
    public static class GhostGizmoDrawer
    {
        private const int LabelFontSize = 24;
        private const float LabelVerticalOffset = 0.5f;
        private static GUIStyle _labelStyle;

        private static GUIStyle LabelStyle => _labelStyle ??= new GUIStyle
        {
            fontSize = LabelFontSize,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };


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

            GridData grid = ghost.GridData;
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
            var content = new GUIContent(ghost.State.ToString());
            Vector3 worldPos = ghost.transform.position + Vector3.up * LabelVerticalOffset;
            Vector2 screenPos = HandleUtility.WorldToGUIPoint(worldPos);
            Vector2 size = LabelStyle.CalcSize(content);
            var rect = new Rect(screenPos.x - size.x * 0.5f, screenPos.y - size.y * 0.5f, size.x, size.y);
            Handles.BeginGUI();
            GUI.Label(rect, content, LabelStyle);
            Handles.EndGUI();
        }
    }
}
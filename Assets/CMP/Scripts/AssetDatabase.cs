using UnityEngine;

namespace CMP.Scripts
{
    public class AssetDatabase
    {
        public static AssetDatabase Instance = new();

        public Pacman PacmanPrefab = Resources.Load<Pacman>("Pacman");
        public InputManager InputManagerPrefab = Resources.Load<InputManager>("InputManager");
        public HudManager HudManagerPrefab = Resources.Load<HudManager>("HudManager");
        public Ghost Ghost = Resources.Load<Ghost>("Ghost");
        public MapVisualSettings MapVisualSettings = Resources.Load<MapVisualSettings>("MapVisualSettings");
        public Collectable PelletPrefab = Resources.Load<Collectable>("Pellet");
        public Collectable PowerPelletPrefab = Resources.Load<Collectable>("PowerPellet");
        public GridData[] Levels = BuildLevels();


        private static GridData[] BuildLevels()
        {
            var extraLevels = Resources.LoadAll<GridData>("Levels");
            System.Array.Sort(extraLevels, (a, b) => string.CompareOrdinal(a.name, b.name));

            var levels = new GridData[extraLevels.Length + 1];
            levels[0] = Resources.Load<GridData>("GridData");
            extraLevels.CopyTo(levels, 1);
            return levels;
        }
    }
}
using System.Collections.Generic;
using CMP.Scripts.Helper;
using UnityEngine;

namespace CMP.Scripts
{
    public enum GameMode
    {
        Scatter,
        Chase,
        GameOver,
    }

    public class GameManager : MonoBehaviour
    {
        private Pacman _pacman;
        private InputManager _inputManager;
        private GameMode _gameMode = GameMode.Scatter;
        private readonly List<Ghost> _ghosts = new();

        public GameMode Mode
        {
            get => _gameMode;
            set => _gameMode = value;
        }

        public Pacman Pacman => _pacman;

        private void Start()
        {
            var gridData = AssetDatabase.Instance.GridData;
            _inputManager = Instantiate(AssetDatabase.Instance.InputManagerPrefab);
            _pacman = Instantiate(AssetDatabase.Instance.PacmanPrefab);
            _pacman.Init(_inputManager, gridData);
            SetupEnemies(gridData);
            CreateBackground(gridData);
            AdjustCamera(gridData);
        }


        #region Setup

        private void SetupEnemies(GridData gridData)
        {
            var spawnPositions = gridData.GetCoordsOfCellType(CellType.AiSpawnZone);

            Debug.Assert(GameSettings.AiCharacterCount <= GameSettings.AiJoinDelays.Length,
                $"AiCharacterCount({GameSettings.AiCharacterCount}) exceeds AiJoinDelays.Length({GameSettings.AiJoinDelays.Length}) length; extra ghosts fall back to an extrapolated stagger.");


            for (int i = 0; i < GameSettings.AiCharacterCount; i++)
            {
                var ghost = Instantiate(AssetDatabase.Instance.Ghost);
                var spawnPos = spawnPositions[i % spawnPositions.Count];
                ghost.Init(gridData, spawnPos, GetJoinDelay(i), this);
                _ghosts.Add(ghost);
            }
        }

        private float GetJoinDelay(int index)
        {
            var delays = GameSettings.AiJoinDelays;
            if (index < delays.Length)
                return delays[index];

            int last = delays.Length - 1;
            float interval = delays[last] - delays[last - 1];
            return delays[last] + (index - last) * interval;
        }


        private void CreateBackground(GridData gridData)
        {
            var targetTexture = MapTextureGenerator.Generate(gridData, AssetDatabase.Instance.MapVisualSettings);
            var textureObject = new GameObject("MapTexture");
            textureObject.transform.position = new Vector3(-0.5f, -0.5f, 0f);
            var targetSprite = Sprite.Create(targetTexture, new Rect(0f, 0f, targetTexture.width, targetTexture.height),
                Vector2.zero, AssetDatabase.Instance.MapVisualSettings.pixelsPerCell);
            var spriteRenderer = textureObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = targetSprite;
            spriteRenderer.sortingOrder = -1;
        }

        private void AdjustCamera(GridData gridData)
        {
            var mainCamera = Camera.main;
            mainCamera.orthographicSize = gridData.Height + GameSettings.CameraPadding;
            mainCamera.transform.position = new Vector3(gridData.Width / 2f - 0.5f, 0f, -10f);
        }

        #endregion


        private void Update()
        {
            if (_gameMode == GameMode.GameOver)
                return;

            foreach (var ghost in _ghosts)
            {
                float distance = Vector2.Distance(ghost.transform.position, _pacman.transform.position);
                if (distance <= GameSettings.CatchDistance)
                {
                    TriggerGameOver();
                    return;
                }
            }
        }

        private void TriggerGameOver()
        {
            _gameMode = GameMode.GameOver;
            _pacman.PlayFail();
            _pacman.enabled = false;
            foreach (var ghost in _ghosts)
            {
                ghost.enabled = false;
            }
        }
    }
}
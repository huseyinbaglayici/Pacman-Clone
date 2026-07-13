using System.Collections.Generic;
using CMP.Scripts.AiStates;
using CMP.Scripts.Helper;
using UnityEngine;

namespace CMP.Scripts
{
    public enum GameMode
    {
        Scatter,
        Chase,
        GameOver,
        Win,
        Frightened
    }

    public class GameManager : MonoBehaviour
    {
        private Pacman _pacman;
        private InputManager _inputManager;
        private HudManager _hudManager;
        private GameMode _gameMode = GameMode.Scatter;
        private float _frightenedTimer;
        private int _ghostsEaten;
        private int _score;
        private int _remainingCollectables;
        private int _lives;
        private readonly List<Ghost> _ghosts = new();
        private readonly Dictionary<Vector2Int, Collectable> _collectables = new();
        [SerializeField] private int startingLives = 3;

        public float FrightenedTimeLeft => _frightenedTimer;

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
            _lives = startingLives;
            _hudManager = Instantiate(AssetDatabase.Instance.HudManagerPrefab);
            _hudManager.SetScore(_score);
            _hudManager.SetLives(_lives);
            _pacman = Instantiate(AssetDatabase.Instance.PacmanPrefab);
            _pacman.Init(_inputManager, gridData);
            SetupEnemies(gridData);
            SetupCollectables(gridData);
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

        private void SetupCollectables(GridData gridData)
        {
            var holder = new GameObject("Collectables").transform;

            foreach (var cell in gridData.GetCoordsOfCellType(CellType.Pellet))
                _collectables[cell] =
                    Instantiate(AssetDatabase.Instance.PelletPrefab, cell.ToWorld(), Quaternion.identity, holder);

            foreach (var cell in gridData.GetCoordsOfCellType(CellType.PowerPellet))
                _collectables[cell] = Instantiate(AssetDatabase.Instance.PowerPelletPrefab, cell.ToWorld(),
                    Quaternion.identity, holder);

            _remainingCollectables = _collectables.Count;
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
            if (_gameMode is GameMode.GameOver or GameMode.Win)
                return;

            if (_gameMode == GameMode.Frightened)
            {
                _frightenedTimer -= Time.deltaTime;
                if (_frightenedTimer <= 0f)
                    _gameMode = GameMode.Scatter;
            }

            HandleCollectables();
            if (_gameMode == GameMode.Win)
                return;

            HandleCatch();
        }

        private void HandleCollectables()
        {
            if (!_collectables.TryGetValue(_pacman.CurrentGridPos, out var collectable) ||
                !collectable.gameObject.activeSelf)
                return;

            collectable.gameObject.SetActive(false);
            _remainingCollectables--;
            AddScore(collectable.Type == CollectableType.Pellet
                ? GameSettings.PelletScore
                : GameSettings.PowerPelletScore);

            if (collectable.Type == CollectableType.PowerPellet)
                EnterFrightened();

            if (_remainingCollectables == 0)
                TriggerWin();
        }

        private void EnterFrightened()
        {
            _gameMode = GameMode.Frightened;
            _frightenedTimer = GameSettings.FrightenedDuration;
            _ghostsEaten = 0;

            foreach (var ghost in _ghosts)
            {
                if (ghost.State == GhostStateType.Frightened)
                    ghost.GetFrightened();
            }
        }

        private void HandleCatch()
        {
            foreach (var ghost in _ghosts)
            {
                if (ghost.State == GhostStateType.Eaten)
                    continue;

                float distance = Vector2.Distance(ghost.transform.position, _pacman.transform.position);
                if (distance > GameSettings.CatchDistance)
                    continue;

                if (ghost.State == GhostStateType.Frightened)
                {
                    AddScore(GameSettings.GhostScores[Mathf.Min(_ghostsEaten, GameSettings.GhostScores.Length - 1)]);
                    _ghostsEaten++;
                    ghost.GetEaten();
                }

                else if (_gameMode != GameMode.Frightened)
                {
                    TriggerGameOver();
                    return;
                }
            }
        }

        private void AddScore(int amount)
        {
            _score += amount;
            _hudManager.SetScore(_score);
        }

        private void TriggerWin()
        {
            _gameMode = GameMode.Win;
            _pacman.enabled = false;
            foreach (var ghost in _ghosts)
            {
                ghost.enabled = false;
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

            _lives--;
            _hudManager.SetLives(_lives);

            Invoke(_lives > 0 ? nameof(Restart) : nameof(FullRestart), GameSettings.RestartDelay);
        }

        private void FullRestart()
        {
            _lives = startingLives;
            _score = 0;
            _hudManager.SetScore(_score);
            _hudManager.SetLives(_lives);
            ResetCollectables();
            Restart();
        }

        private void ResetCollectables()
        {
            foreach (var collectable in _collectables.Values)
                collectable.gameObject.SetActive(true);

            _remainingCollectables = _collectables.Count;
        }

        private void Restart()
        {
            _gameMode = GameMode.Scatter;
            _inputManager.Clear();
            _frightenedTimer = 0;
            _pacman.Spawn();
            foreach (var ghost in _ghosts)
            {
                ghost.Spawn();
            }
        }
    }
}
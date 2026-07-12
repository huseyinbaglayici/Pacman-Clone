namespace CMP.Scripts
{
    public static class GameSettings
    {
        public const float AiMovementDuration = 0.25f;
        public const float PacmanMovementDuration = 0.25f;
        public const float RestartDelay = 3f;
        public const float FrightenedDuration = 8f;
        public const float FrightenedBlinkWarning = 3f;
        public const float EatenMovementDuration = 0.125f;
        public const int AiCharacterCount = 3;
        public const int AiSightRange = 6;
        public const int PelletScore = 10;
        public const int PowerPelletScore = 50;
        public static readonly float[] AiJoinDelays = { 3f, 6f, 9f };
        public static float CatchDistance = 1f;

        public static readonly Direction[] DirectionsToCheck =
            { Direction.Left, Direction.Right, Direction.Up, Direction.Down };

        public static readonly int[] GhostScores = { 200, 400, 800, 1600 };

        public static float CameraPadding = 1f;
    }
}
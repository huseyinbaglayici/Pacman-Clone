using System.Collections.Generic;

namespace CMP.Scripts.AiStates
{
    public static class GhostCells
    {
        public static readonly List<CellType> GateTransit = new()
        {
            CellType.AiSpawnZone, CellType.Empty, CellType.AiGate,
            CellType.JoinGameCell, CellType.Pellet, CellType.PowerPellet
        };
    }
}
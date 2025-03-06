namespace Critsoft.CozyShip.Gameplay
{
    public class PlayerStatsEventArgs
    {
        public int Points { get; }
        public int Collisions { get; }
        public float ElapsedTime { get; }

        public PlayerStatsEventArgs(int points, int collisions, float elapsedTime)
        {
            Points = points;
            Collisions = collisions;
            ElapsedTime = elapsedTime;
        }
    }
}

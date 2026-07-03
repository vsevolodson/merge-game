namespace Gameplay
{
    public class ItemData
    {
        public int Level { get; private set; } = 1;

        public void Upgrade()
        {
            Level++;
        }
    }
}
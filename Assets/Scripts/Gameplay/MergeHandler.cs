namespace Gameplay
{
    public class MergeHandler
    {
        public ItemData GetMergeResult(ItemData first, ItemData second)
        {
            if (first == null || second == null)
            {
                return null;
            }

            if (first.Type != second.Type)
            {
                return null;
            }

            if (first.Level != second.Level)
            {
                return null;
            }

            return first.NextLevel;
        }
    }
}
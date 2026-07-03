namespace Gameplay
{
    public class MergeHandler
    {
        public ItemData TryMerge(ItemData first, ItemData second)
        {
            if (first == null || second == null)
            {
                return null;
            }

            if (first != second)
            {
                return null;
            }

            return first.NextLevel;
        }
    }
}
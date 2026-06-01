namespace Match3.Core.Utilities
{
    public static class ArrayExtensions
    {
        public static T Random<T>(this T[] array)
        {
            var randomNumber = UnityEngine.Random.Range(0, array.Length);
            return array[randomNumber];
        }
    }
}
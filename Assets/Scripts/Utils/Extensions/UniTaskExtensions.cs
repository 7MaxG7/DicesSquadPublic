using Cysharp.Threading.Tasks;

namespace Utils.Extensions
{
    public static class UniTaskExtensions
    {
        public static void Update<T>(this AsyncReactiveProperty<T> property, T value)
        {
            if (property.Value.Equals(value))
                return;

            property.Value = value;
        }

    }
}
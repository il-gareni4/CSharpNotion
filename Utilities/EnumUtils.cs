namespace CSharpNotion.Utilities
{
    internal sealed class EnumUtils
    {
        private static Random _random = new();

        private EnumUtils()
        { }

        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(_random.Next(v.Length))!;
        }
    }
}
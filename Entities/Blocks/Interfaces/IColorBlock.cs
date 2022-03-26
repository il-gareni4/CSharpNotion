namespace CSharpNotion.Entities.Blocks.Interfaces
{
    public enum BlockColor
    {
        Default,
        Gray,
        Brown,
        Orange,
        Yellow,
        Green,
        Blue,
        Purple,
        Pink,
        Red,
        GrayBackground,
        BrownBackground,
        OrangeBackground,
        YellowBackground,
        GreenBackground,
        BlueBackground,
        PurpleBackground,
        PinkBackground,
        RedBackground
    }

    public static class BlockColorExtensions
    {
        public static string? ToColorString(this BlockColor blockColor)
        {
            if (blockColor == BlockColor.Default) return null;
            else if (blockColor == BlockColor.Green) return "teal";
            else if (blockColor == BlockColor.GreenBackground) return "teal_background";

            string stringBlockColor = blockColor.ToString();
            if (stringBlockColor.Contains("Background")) return stringBlockColor[..^10].ToLower() + "_" + stringBlockColor[^10..].ToLower();
            else return stringBlockColor.ToLower();
        }

        public static BlockColor ToBlockColor(string? stringBlockColor)
        {
            if (string.IsNullOrEmpty(stringBlockColor)) return BlockColor.Default;
            else if (stringBlockColor == "teal") return BlockColor.Green;
            else if (stringBlockColor == "teal_background") return BlockColor.GreenBackground;

            string TitleCaseStringBlockColor = string.Join("", stringBlockColor.Split('_').Select((word) => char.ToUpper(word[0]) + word[1..]));
            return Enum.Parse<BlockColor>(TitleCaseStringBlockColor);
        }
    }

    public interface IColorBlock<T> where T : BaseBlock
    {
        BlockColor Color { get; }

        T SetColor(BlockColor color);
    }
}
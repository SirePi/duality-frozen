// This code is provided under the MIT license. Originally by Alessandro Pilati.

namespace FrozenCore.Widgets
{
    public enum ActiveArea
    {
        None,
        Custom,
        LeftBorder,
        TopBorder,
        RightBorder,
        BottomBorder,
        Center,
        All
    }

    /*
     * Maybe in the future...
    [Flags]
    public enum ActiveArea
    {
        None = 0x0000,
        Custom = 0x0001,
        TopLeft = 0x0002,
        TopCenter = 0x0004,
        TopRight = 0x0008,
        CenterLeft = 0x0010,
        Center = 0x0020,
        CenterRight = 0x0040,
        BottomLeft = 0x0080,
        BottomCenter = 0x0100,
        BottomRight = 0x0200,
        Left = TopLeft | CenterLeft | BottomLeft,
        Top = TopLeft | TopCenter | TopRight,
        Right = TopRight | CenterRight | BottomRight,
        Bottom = BottomLeft | BottomCenter | BottomRight,
        All = Left | Top | Right | Bottom | Center
    }
     * */
}
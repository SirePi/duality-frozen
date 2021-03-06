﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Drawing;

namespace SnowyPeak.Duality.Plugin.Frozen.Core
{
#pragma warning disable 1591

    /// <summary>
    /// Utility class containing the translation of .net's default Colors to ColorRgba objects
    /// </summary>
    public static class Colors
    {
        public static readonly ColorRgba AliceBlue = new ColorRgba(240, 248, 255, 255);
        public static readonly ColorRgba AntiqueWhite = new ColorRgba(250, 235, 215, 255);
        public static readonly ColorRgba Aqua = new ColorRgba(0, 255, 255, 255);
        public static readonly ColorRgba Aquamarine = new ColorRgba(127, 255, 212, 255);
        public static readonly ColorRgba Azure = new ColorRgba(240, 255, 255, 255);
        public static readonly ColorRgba Beige = new ColorRgba(245, 245, 220, 255);
        public static readonly ColorRgba Bisque = new ColorRgba(255, 228, 196, 255);
        public static readonly ColorRgba Black = new ColorRgba(0, 0, 0, 255);
        public static readonly ColorRgba BlanchedAlmond = new ColorRgba(255, 235, 205, 255);
        public static readonly ColorRgba Blue = new ColorRgba(0, 0, 255, 255);
        public static readonly ColorRgba BlueViolet = new ColorRgba(138, 43, 226, 255);
        public static readonly ColorRgba Brown = new ColorRgba(165, 42, 42, 255);
        public static readonly ColorRgba BurlyWood = new ColorRgba(222, 184, 135, 255);
        public static readonly ColorRgba CadetBlue = new ColorRgba(95, 158, 160, 255);
        public static readonly ColorRgba Chartreuse = new ColorRgba(127, 255, 0, 255);
        public static readonly ColorRgba Chocolate = new ColorRgba(210, 105, 30, 255);
        public static readonly ColorRgba Coral = new ColorRgba(255, 127, 80, 255);
        public static readonly ColorRgba CornflowerBlue = new ColorRgba(100, 149, 237, 255);
        public static readonly ColorRgba Cornsilk = new ColorRgba(255, 248, 220, 255);
        public static readonly ColorRgba Crimson = new ColorRgba(220, 20, 60, 255);
        public static readonly ColorRgba Cyan = new ColorRgba(0, 255, 255, 255);
        public static readonly ColorRgba DarkBlue = new ColorRgba(0, 0, 139, 255);
        public static readonly ColorRgba DarkCyan = new ColorRgba(0, 139, 139, 255);
        public static readonly ColorRgba DarkGoldenrod = new ColorRgba(184, 134, 11, 255);
        public static readonly ColorRgba DarkGray = new ColorRgba(169, 169, 169, 255);
        public static readonly ColorRgba DarkGreen = new ColorRgba(0, 100, 0, 255);
        public static readonly ColorRgba DarkKhaki = new ColorRgba(189, 183, 107, 255);
        public static readonly ColorRgba DarkMagenta = new ColorRgba(139, 0, 139, 255);
        public static readonly ColorRgba DarkOliveGreen = new ColorRgba(85, 107, 47, 255);
        public static readonly ColorRgba DarkOrange = new ColorRgba(255, 140, 0, 255);
        public static readonly ColorRgba DarkOrchid = new ColorRgba(153, 50, 204, 255);
        public static readonly ColorRgba DarkRed = new ColorRgba(139, 0, 0, 255);
        public static readonly ColorRgba DarkSalmon = new ColorRgba(233, 150, 122, 255);
        public static readonly ColorRgba DarkSeaGreen = new ColorRgba(143, 188, 139, 255);
        public static readonly ColorRgba DarkSlateBlue = new ColorRgba(72, 61, 139, 255);
        public static readonly ColorRgba DarkSlateGray = new ColorRgba(47, 79, 79, 255);
        public static readonly ColorRgba DarkTurquoise = new ColorRgba(0, 206, 209, 255);
        public static readonly ColorRgba DarkViolet = new ColorRgba(148, 0, 211, 255);
        public static readonly ColorRgba DeepPink = new ColorRgba(255, 20, 147, 255);
        public static readonly ColorRgba DeepSkyBlue = new ColorRgba(0, 191, 255, 255);
        public static readonly ColorRgba DimGray = new ColorRgba(105, 105, 105, 255);
        public static readonly ColorRgba DodgerBlue = new ColorRgba(30, 144, 255, 255);
        public static readonly ColorRgba Firebrick = new ColorRgba(178, 34, 34, 255);
        public static readonly ColorRgba FloralWhite = new ColorRgba(255, 250, 240, 255);
        public static readonly ColorRgba ForestGreen = new ColorRgba(34, 139, 34, 255);
        public static readonly ColorRgba Fuchsia = new ColorRgba(255, 0, 255, 255);
        public static readonly ColorRgba Gainsboro = new ColorRgba(220, 220, 220, 255);
        public static readonly ColorRgba GhostWhite = new ColorRgba(248, 248, 255, 255);
        public static readonly ColorRgba Gold = new ColorRgba(255, 215, 0, 255);
        public static readonly ColorRgba Goldenrod = new ColorRgba(218, 165, 32, 255);
        public static readonly ColorRgba Gray = new ColorRgba(128, 128, 128, 255);
        public static readonly ColorRgba Green = new ColorRgba(0, 128, 0, 255);
        public static readonly ColorRgba GreenYellow = new ColorRgba(173, 255, 47, 255);
        public static readonly ColorRgba Honeydew = new ColorRgba(240, 255, 240, 255);
        public static readonly ColorRgba HotPink = new ColorRgba(255, 105, 180, 255);
        public static readonly ColorRgba IndianRed = new ColorRgba(205, 92, 92, 255);
        public static readonly ColorRgba Indigo = new ColorRgba(75, 0, 130, 255);
        public static readonly ColorRgba Ivory = new ColorRgba(255, 255, 240, 255);
        public static readonly ColorRgba Khaki = new ColorRgba(240, 230, 140, 255);
        public static readonly ColorRgba Lavender = new ColorRgba(230, 230, 250, 255);
        public static readonly ColorRgba LavenderBlush = new ColorRgba(255, 240, 245, 255);
        public static readonly ColorRgba LawnGreen = new ColorRgba(124, 252, 0, 255);
        public static readonly ColorRgba LemonChiffon = new ColorRgba(255, 250, 205, 255);
        public static readonly ColorRgba LightBlue = new ColorRgba(173, 216, 230, 255);
        public static readonly ColorRgba LightCoral = new ColorRgba(240, 128, 128, 255);
        public static readonly ColorRgba LightCyan = new ColorRgba(224, 255, 255, 255);
        public static readonly ColorRgba LightGoldenrodYellow = new ColorRgba(250, 250, 210, 255);
        public static readonly ColorRgba LightGray = new ColorRgba(211, 211, 211, 255);
        public static readonly ColorRgba LightGreen = new ColorRgba(144, 238, 144, 255);
        public static readonly ColorRgba LightPink = new ColorRgba(255, 182, 193, 255);
        public static readonly ColorRgba LightSalmon = new ColorRgba(255, 160, 122, 255);
        public static readonly ColorRgba LightSeaGreen = new ColorRgba(32, 178, 170, 255);
        public static readonly ColorRgba LightSkyBlue = new ColorRgba(135, 206, 250, 255);
        public static readonly ColorRgba LightSlateGray = new ColorRgba(119, 136, 153, 255);
        public static readonly ColorRgba LightSteelBlue = new ColorRgba(176, 196, 222, 255);
        public static readonly ColorRgba LightYellow = new ColorRgba(255, 255, 224, 255);
        public static readonly ColorRgba Lime = new ColorRgba(0, 255, 0, 255);
        public static readonly ColorRgba LimeGreen = new ColorRgba(50, 205, 50, 255);
        public static readonly ColorRgba Linen = new ColorRgba(250, 240, 230, 255);
        public static readonly ColorRgba Magenta = new ColorRgba(255, 0, 255, 255);
        public static readonly ColorRgba Maroon = new ColorRgba(128, 0, 0, 255);
        public static readonly ColorRgba MediumAquamarine = new ColorRgba(102, 205, 170, 255);
        public static readonly ColorRgba MediumBlue = new ColorRgba(0, 0, 205, 255);
        public static readonly ColorRgba MediumOrchid = new ColorRgba(186, 85, 211, 255);
        public static readonly ColorRgba MediumPurple = new ColorRgba(147, 112, 219, 255);
        public static readonly ColorRgba MediumSeaGreen = new ColorRgba(60, 179, 113, 255);
        public static readonly ColorRgba MediumSlateBlue = new ColorRgba(123, 104, 238, 255);
        public static readonly ColorRgba MediumSpringGreen = new ColorRgba(0, 250, 154, 255);
        public static readonly ColorRgba MediumTurquoise = new ColorRgba(72, 209, 204, 255);
        public static readonly ColorRgba MediumVioletRed = new ColorRgba(199, 21, 133, 255);
        public static readonly ColorRgba MidnightBlue = new ColorRgba(25, 25, 112, 255);
        public static readonly ColorRgba MintCream = new ColorRgba(245, 255, 250, 255);
        public static readonly ColorRgba MistyRose = new ColorRgba(255, 228, 225, 255);
        public static readonly ColorRgba Moccasin = new ColorRgba(255, 228, 181, 255);
        public static readonly ColorRgba NavajoWhite = new ColorRgba(255, 222, 173, 255);
        public static readonly ColorRgba Navy = new ColorRgba(0, 0, 128, 255);
        public static readonly ColorRgba OldLace = new ColorRgba(253, 245, 230, 255);
        public static readonly ColorRgba Olive = new ColorRgba(128, 128, 0, 255);
        public static readonly ColorRgba OliveDrab = new ColorRgba(107, 142, 35, 255);
        public static readonly ColorRgba Orange = new ColorRgba(255, 165, 0, 255);
        public static readonly ColorRgba OrangeRed = new ColorRgba(255, 69, 0, 255);
        public static readonly ColorRgba Orchid = new ColorRgba(218, 112, 214, 255);
        public static readonly ColorRgba PaleGoldenrod = new ColorRgba(238, 232, 170, 255);
        public static readonly ColorRgba PaleGreen = new ColorRgba(152, 251, 152, 255);
        public static readonly ColorRgba PaleTurquoise = new ColorRgba(175, 238, 238, 255);
        public static readonly ColorRgba PaleVioletRed = new ColorRgba(219, 112, 147, 255);
        public static readonly ColorRgba PapayaWhip = new ColorRgba(255, 239, 213, 255);
        public static readonly ColorRgba PeachPuff = new ColorRgba(255, 218, 185, 255);
        public static readonly ColorRgba Peru = new ColorRgba(205, 133, 63, 255);
        public static readonly ColorRgba Pink = new ColorRgba(255, 192, 203, 255);
        public static readonly ColorRgba Plum = new ColorRgba(221, 160, 221, 255);
        public static readonly ColorRgba PowderBlue = new ColorRgba(176, 224, 230, 255);
        public static readonly ColorRgba Purple = new ColorRgba(128, 0, 128, 255);
        public static readonly ColorRgba Red = new ColorRgba(255, 0, 0, 255);
        public static readonly ColorRgba RosyBrown = new ColorRgba(188, 143, 143, 255);
        public static readonly ColorRgba RoyalBlue = new ColorRgba(65, 105, 225, 255);
        public static readonly ColorRgba SaddleBrown = new ColorRgba(139, 69, 19, 255);
        public static readonly ColorRgba Salmon = new ColorRgba(250, 128, 114, 255);
        public static readonly ColorRgba SandyBrown = new ColorRgba(244, 164, 96, 255);
        public static readonly ColorRgba SeaGreen = new ColorRgba(46, 139, 87, 255);
        public static readonly ColorRgba SeaShell = new ColorRgba(255, 245, 238, 255);
        public static readonly ColorRgba Sienna = new ColorRgba(160, 82, 45, 255);
        public static readonly ColorRgba Silver = new ColorRgba(192, 192, 192, 255);
        public static readonly ColorRgba SkyBlue = new ColorRgba(135, 206, 235, 255);
        public static readonly ColorRgba SlateBlue = new ColorRgba(106, 90, 205, 255);
        public static readonly ColorRgba SlateGray = new ColorRgba(112, 128, 144, 255);
        public static readonly ColorRgba Snow = new ColorRgba(255, 250, 250, 255);
        public static readonly ColorRgba SpringGreen = new ColorRgba(0, 255, 127, 255);
        public static readonly ColorRgba SteelBlue = new ColorRgba(70, 130, 180, 255);
        public static readonly ColorRgba Tan = new ColorRgba(210, 180, 140, 255);
        public static readonly ColorRgba Teal = new ColorRgba(0, 128, 128, 255);
        public static readonly ColorRgba Thistle = new ColorRgba(216, 191, 216, 255);
        public static readonly ColorRgba Tomato = new ColorRgba(255, 99, 71, 255);
        public static readonly ColorRgba Transparent = new ColorRgba(0, 0, 0, 0);
        public static readonly ColorRgba TransparentWhite = new ColorRgba(255, 255, 255, 0);
        public static readonly ColorRgba Turquoise = new ColorRgba(64, 224, 208, 255);
        public static readonly ColorRgba Violet = new ColorRgba(238, 130, 238, 255);
        public static readonly ColorRgba Wheat = new ColorRgba(245, 222, 179, 255);
        public static readonly ColorRgba White = new ColorRgba(255, 255, 255, 255);
        public static readonly ColorRgba WhiteSmoke = new ColorRgba(245, 245, 245, 255);
        public static readonly ColorRgba Yellow = new ColorRgba(255, 255, 0, 255);
        public static readonly ColorRgba YellowGreen = new ColorRgba(154, 205, 50, 255);
#pragma warning restore 1591

        /// <summary>
        /// Converts a Vector4 with values between 0 and 1 to ColorRgba
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ColorRgba FromBase1Vector4(Vector4 value)
        {
            return new ColorRgba(value.X, value.Y, value.Z, value.W);
        }

        /// <summary>
        /// Converts a Vector4 with values between 0 and 255 to ColorRgba
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ColorRgba FromBase255Vector4(Vector4 value)
        {
            return new ColorRgba((byte)value.X, (byte)value.Y, (byte)value.Z, (byte)value.W);
        }
    }
}
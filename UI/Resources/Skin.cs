// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Widgets;
using System.Collections.Generic;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Resources
{
	/// <summary>
	///
	/// </summary>
	[EditorHintCategory(ResNames.CategoryWidgets)]
	[EditorHintImage(ResNames.ImageSkin)]
	public class Skin : Resource
	{
#pragma warning disable 1591
		[DontSerialize]
		protected static readonly int[] COLUMN_1 = new int[] { 0, 1, 12, 13, 24, 25 };

		[DontSerialize]
		protected static readonly int[] COLUMN_2 = new int[] { 3, 2, 15, 14, 27, 26 };

		[DontSerialize]
		protected static readonly int[] COLUMN_3 = new int[] { 4, 5, 16, 17, 28, 29 };

		[DontSerialize]
		protected static readonly int[] COLUMN_4 = new int[] { 7, 6, 19, 18, 31, 30 };

		[DontSerialize]
		protected static readonly int[] COLUMN_5 = new int[] { 8, 9, 20, 21, 32, 33 };

		[DontSerialize]
		protected static readonly int[] COLUMN_6 = new int[] { 11, 10, 23, 22, 35, 34 };

		[DontSerialize]
		protected static readonly int[] ROW_1 = new int[] { 0, 3, 4, 7, 8, 11 };

		[DontSerialize]
		protected static readonly int[] ROW_2 = new int[] { 1, 2, 5, 6, 9, 10 };

		[DontSerialize]
		protected static readonly int[] ROW_3 = new int[] { 12, 15, 16, 19, 20, 23 };

		[DontSerialize]
		protected static readonly int[] ROW_4 = new int[] { 13, 14, 17, 18, 21, 22 };

		[DontSerialize]
		protected static readonly int[] ROW_5 = new int[] { 24, 27, 28, 31, 32, 35 };

		[DontSerialize]
		protected static readonly int[] ROW_6 = new int[] { 25, 26, 29, 30, 33, 34 };
#pragma warning restore 1591

		/// <summary>
		/// [GET / SET] The Material used by this Skin
		/// </summary>
		public ContentRef<Material> Material { get; set; }

		/// <summary>
		/// [GET / SET] The list of appearances available for the Skin
		/// </summary>
		public Dictionary<string, Appearance> WidgetAppearances { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public Skin()
		{
			Material = global::Duality.Resources.Material.Checkerboard;
			WidgetAppearances = new Dictionary<string, Appearance>();

			WidgetAppearances.Add("Bar", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0});
			WidgetAppearances.Add("Button", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("CheckButton", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("CheckButtonGlyph", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("CommandGrid", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("DropDownButton", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("Highlight", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("ListBox", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("Panel", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("ProgressBar", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("RadioButton", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("RadioButtonGlyph", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("ScrollBar", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("ScrollBarCursor", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("ScrollBarDecreaseButton", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("ScrollBarIncreaseButton", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("TextBlock", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("TextBox", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("Window", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("WindowCloseButton", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("WindowMaximizeButton", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("WindowMinimizeButton", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
			WidgetAppearances.Add("WindowRestoreButton", new Appearance() { Border = Vector4.Zero, Normal = 0, Hover = 0, Active = 0, Disabled = 0 });
		}

		/// <summary>
		/// Prepares the Widget vertices based on the desired appearance
		/// </summary>
		/// <param name="vertices"></param>
		/// <param name="widgetArea"></param>
		/// <param name="scale"></param>
		/// <param name="appearanceName"></param>
		/// <param name="status"></param>
		public void PrepareVertices(ref MultiSpacePoint[] vertices, Rect widgetArea, float scale, string appearanceName, Widget.WidgetStatus status)
		{
			Appearance appearance = WidgetAppearances[appearanceName];
			Vector4 border = appearance.Border;

			Rect waTemp = widgetArea.Transformed(scale, scale);

			Vector2 topLeft = waTemp.TopLeft;
			Vector2 bottomRight = waTemp.BottomRight;

			Vector2 innerTopLeft = topLeft + (border.XY() * scale);
			Vector2 innerBottomRight = bottomRight - (border.ZW() * scale);

			Vector2 size = bottomRight - topLeft;

			vertices[0].SceneCoords.X = topLeft.X;
			vertices[0].SceneCoords.Y = topLeft.Y;

			vertices[1].SceneCoords.X = innerTopLeft.X;
			vertices[1].SceneCoords.Y = topLeft.Y;

			vertices[2].SceneCoords.X = innerBottomRight.X;
			vertices[2].SceneCoords.Y = topLeft.Y;

			vertices[3].SceneCoords.X = bottomRight.X;
			vertices[3].SceneCoords.Y = topLeft.Y;

			vertices[4].SceneCoords.X = topLeft.X;
			vertices[4].SceneCoords.Y = innerTopLeft.Y;

			vertices[5].SceneCoords.X = innerTopLeft.X;
			vertices[5].SceneCoords.Y = innerTopLeft.Y;

			vertices[6].SceneCoords.X = innerBottomRight.X;
			vertices[6].SceneCoords.Y = innerTopLeft.Y;

			vertices[7].SceneCoords.X = bottomRight.X;
			vertices[7].SceneCoords.Y = innerTopLeft.Y;

			vertices[8].SceneCoords.X = topLeft.X;
			vertices[8].SceneCoords.Y = innerBottomRight.Y;

			vertices[9].SceneCoords.X = innerTopLeft.X;
			vertices[9].SceneCoords.Y = innerBottomRight.Y;

			vertices[10].SceneCoords.X = innerBottomRight.X;
			vertices[10].SceneCoords.Y = innerBottomRight.Y;

			vertices[11].SceneCoords.X = bottomRight.X;
			vertices[11].SceneCoords.Y = innerBottomRight.Y;

			vertices[12].SceneCoords.X = topLeft.X;
			vertices[12].SceneCoords.Y = bottomRight.Y;

			vertices[13].SceneCoords.X = innerTopLeft.X;
			vertices[13].SceneCoords.Y = bottomRight.Y;

			vertices[14].SceneCoords.X = innerBottomRight.X;
			vertices[14].SceneCoords.Y = bottomRight.Y;

			vertices[15].SceneCoords.X = bottomRight.X;
			vertices[15].SceneCoords.Y = bottomRight.Y;

			Texture tx = Material.Res.MainTexture.Res;
			Rect txRect = tx.LookupAtlas(appearance.GetAtlasForStatus(status));

			Vector2 txSize = tx.Size * txRect.Size;

			float borderLeft = border.X / txSize.X * txRect.W;
			float borderTop = border.Y / txSize.Y * txRect.H;
			float borderRight = txRect.W - (border.Z / txSize.X * txRect.W);
			float borderBottom = txRect.H - (border.W / txSize.Y * txRect.H);

			vertices[0].UVCoords = txRect.TopLeft + Vector2.Zero;
			vertices[0].Tint = ColorRgba.White;
			vertices[1].UVCoords = txRect.TopLeft + new Vector2(borderLeft, 0);
			vertices[1].Tint = ColorRgba.White;
			vertices[2].UVCoords = txRect.TopLeft + new Vector2(borderRight, 0);
			vertices[2].Tint = ColorRgba.White;
			vertices[3].UVCoords = txRect.TopLeft + (Vector2.UnitX * txRect.Size);
			vertices[3].Tint = ColorRgba.White;
			vertices[4].UVCoords = txRect.TopLeft + new Vector2(0, borderTop);
			vertices[4].Tint = ColorRgba.White;
			vertices[5].UVCoords = txRect.TopLeft + new Vector2(borderLeft, borderTop);
			vertices[5].Tint = ColorRgba.White;
			vertices[6].UVCoords = txRect.TopLeft + new Vector2(borderRight, borderTop);
			vertices[6].Tint = ColorRgba.White;
			vertices[7].UVCoords = txRect.TopLeft + new Vector2(txRect.Size.X, borderTop);
			vertices[7].Tint = ColorRgba.White;
			vertices[8].UVCoords = txRect.TopLeft + new Vector2(0, borderBottom);
			vertices[8].Tint = ColorRgba.White;
			vertices[9].UVCoords = txRect.TopLeft + new Vector2(borderLeft, borderBottom);
			vertices[9].Tint = ColorRgba.White;
			vertices[10].UVCoords = txRect.TopLeft + new Vector2(borderRight, borderBottom);
			vertices[10].Tint = ColorRgba.White;
			vertices[11].UVCoords = txRect.TopLeft + new Vector2(txRect.Size.X, borderBottom);
			vertices[11].Tint = ColorRgba.White;
			vertices[12].UVCoords = txRect.TopLeft + (Vector2.UnitY * txRect.Size);
			vertices[12].Tint = ColorRgba.White;
			vertices[13].UVCoords = txRect.TopLeft + new Vector2(borderLeft, txRect.Size.Y);
			vertices[13].Tint = ColorRgba.White;
			vertices[14].UVCoords = txRect.TopLeft + new Vector2(borderRight, txRect.Size.Y);
			vertices[14].Tint = ColorRgba.White;
			vertices[15].UVCoords = txRect.TopLeft + (Vector2.One * txRect.Size);
			vertices[15].Tint = ColorRgba.White;
		}

		internal void Clip(ref VertexC1P3T2[] vertices, Rect widgetArea, Rect clipArea, string appearanceName)
		{
			Appearance appearance = WidgetAppearances[appearanceName];
			Vector4 border = appearance.Border;

			Vector2 topLeft = widgetArea.TopLeft;
			Vector2 bottomRight = widgetArea.BottomRight;

			Vector2 innerTopLeft = topLeft + (border.XY());
			Vector2 innerBottomRight = bottomRight - (border.ZW());

			//applying clipping area
			if (clipArea != Rect.Empty && clipArea != widgetArea)
			{
				if (clipArea.LeftX >= innerTopLeft.X)
				{
					vertices[0].Color = Colors.Transparent;
					vertices[1].Color = Colors.Transparent;
					vertices[2].Color = Colors.Transparent;
					vertices[3].Color = Colors.Transparent;
					vertices[12].Color = Colors.Transparent;
					vertices[13].Color = Colors.Transparent;
					vertices[14].Color = Colors.Transparent;
					vertices[15].Color = Colors.Transparent;
					vertices[24].Color = Colors.Transparent;
					vertices[25].Color = Colors.Transparent;
					vertices[26].Color = Colors.Transparent;
					vertices[27].Color = Colors.Transparent;
				}

				if (clipArea.RightX <= innerBottomRight.X)
				{
					vertices[8].Color = Colors.Transparent;
					vertices[9].Color = Colors.Transparent;
					vertices[10].Color = Colors.Transparent;
					vertices[11].Color = Colors.Transparent;
					vertices[20].Color = Colors.Transparent;
					vertices[21].Color = Colors.Transparent;
					vertices[22].Color = Colors.Transparent;
					vertices[23].Color = Colors.Transparent;
					vertices[32].Color = Colors.Transparent;
					vertices[33].Color = Colors.Transparent;
					vertices[34].Color = Colors.Transparent;
					vertices[35].Color = Colors.Transparent;
				}

				if ((clipArea.LeftX >= innerBottomRight.X) || (clipArea.RightX <= innerTopLeft.X))
				{
					vertices[4].Color = Colors.Transparent;
					vertices[5].Color = Colors.Transparent;
					vertices[6].Color = Colors.Transparent;
					vertices[7].Color = Colors.Transparent;
					vertices[16].Color = Colors.Transparent;
					vertices[17].Color = Colors.Transparent;
					vertices[18].Color = Colors.Transparent;
					vertices[19].Color = Colors.Transparent;
					vertices[28].Color = Colors.Transparent;
					vertices[29].Color = Colors.Transparent;
					vertices[30].Color = Colors.Transparent;
					vertices[31].Color = Colors.Transparent;
				}

				if (clipArea.TopY >= innerTopLeft.Y)
				{
					vertices[0].Color = Colors.Transparent;
					vertices[1].Color = Colors.Transparent;
					vertices[2].Color = Colors.Transparent;
					vertices[3].Color = Colors.Transparent;
					vertices[4].Color = Colors.Transparent;
					vertices[5].Color = Colors.Transparent;
					vertices[6].Color = Colors.Transparent;
					vertices[7].Color = Colors.Transparent;
					vertices[8].Color = Colors.Transparent;
					vertices[9].Color = Colors.Transparent;
					vertices[10].Color = Colors.Transparent;
					vertices[11].Color = Colors.Transparent;
				}

				if (clipArea.BottomY <= innerBottomRight.Y)
				{
					vertices[24].Color = Colors.Transparent;
					vertices[25].Color = Colors.Transparent;
					vertices[26].Color = Colors.Transparent;
					vertices[27].Color = Colors.Transparent;
					vertices[28].Color = Colors.Transparent;
					vertices[29].Color = Colors.Transparent;
					vertices[30].Color = Colors.Transparent;
					vertices[31].Color = Colors.Transparent;
					vertices[32].Color = Colors.Transparent;
					vertices[33].Color = Colors.Transparent;
					vertices[34].Color = Colors.Transparent;
					vertices[35].Color = Colors.Transparent;
				}

				if ((clipArea.TopY >= innerBottomRight.Y) || (clipArea.BottomY <= innerTopLeft.Y))
				{
					vertices[12].Color = Colors.Transparent;
					vertices[13].Color = Colors.Transparent;
					vertices[14].Color = Colors.Transparent;
					vertices[15].Color = Colors.Transparent;
					vertices[16].Color = Colors.Transparent;
					vertices[17].Color = Colors.Transparent;
					vertices[18].Color = Colors.Transparent;
					vertices[19].Color = Colors.Transparent;
					vertices[20].Color = Colors.Transparent;
					vertices[21].Color = Colors.Transparent;
					vertices[22].Color = Colors.Transparent;
					vertices[23].Color = Colors.Transparent;
				}

				Vector2 centerSize = innerBottomRight - innerTopLeft;

				// Checking Left side
				if (clipArea.TopLeft.X < innerTopLeft.X)
				{
					float k = clipArea.TopLeft.X / border.X;
					FixVertices(ref vertices, COLUMN_1, 3, k);
				}
				else if (clipArea.TopLeft.X < innerBottomRight.X)
				{
					float k = (clipArea.TopLeft.X - innerTopLeft.X) / (centerSize.X);
					FixVertices(ref vertices, COLUMN_3, 7, k);
				}
				else
				{
					float k = (clipArea.TopLeft.X - innerBottomRight.X) / border.W;
					FixVertices(ref vertices, COLUMN_5, 11, k);
				}

				// Checking Right side
				if (clipArea.BottomRight.X > innerBottomRight.X)
				{
					float k = (bottomRight.X - clipArea.BottomRight.X) / border.W;
					FixVertices(ref vertices, COLUMN_6, 8, k);
				}
				else if (clipArea.BottomRight.X > innerTopLeft.X)
				{
					float k = (innerBottomRight.X - clipArea.BottomRight.X) / (centerSize.X);
					FixVertices(ref vertices, COLUMN_4, 4, k);
				}
				else
				{
					float k = (topLeft.X - clipArea.BottomRight.X) / border.X;
					FixVertices(ref vertices, COLUMN_2, 0, k);
				}

				// Checking Top side
				if (clipArea.TopLeft.Y < innerTopLeft.Y)
				{
					float k = clipArea.TopLeft.Y / border.Y;
					FixVertices(ref vertices, ROW_1, 1, k);
				}
				else if (clipArea.TopLeft.Y < innerBottomRight.Y)
				{
					float k = (clipArea.TopLeft.Y - innerTopLeft.Y) / (centerSize.Y);
					FixVertices(ref vertices, ROW_3, 13, k);
				}
				else
				{
					float k = (clipArea.TopLeft.Y - innerBottomRight.Y) / border.Z;
					FixVertices(ref vertices, ROW_5, 25, k);
				}

				// Checking Bottom side
				if (clipArea.BottomRight.Y > innerBottomRight.Y)
				{
					float k = (bottomRight.Y - clipArea.BottomRight.Y) / border.Z;
					FixVertices(ref vertices, ROW_6, 24, k);
				}
				else if (clipArea.BottomRight.Y > innerTopLeft.Y)
				{
					float k = (innerBottomRight.Y - clipArea.BottomRight.Y) / (centerSize.Y);
					FixVertices(ref vertices, ROW_4, 12, k);
				}
				else
				{
					float k = (topLeft.Y - clipArea.BottomRight.Y) / border.Y;
					FixVertices(ref vertices, ROW_2, 0, k);
				}
			}
		}

		private void FixVertices(ref VertexC1P3T2[] vertices, int[] inVertexIndexes, int inVertexComparator, float inK)
		{
			Vector3 deltaPos = (vertices[inVertexComparator].Pos - vertices[inVertexIndexes[0]].Pos) * inK;
			Vector2 deltaTex = (vertices[inVertexComparator].TexCoord - vertices[inVertexIndexes[0]].TexCoord) * inK;
			Vector4 deltaColor = (vertices[inVertexComparator].Color.ToVector4() - vertices[inVertexIndexes[0]].Color.ToVector4()) * inK;

			foreach (int i in inVertexIndexes)
			{
				vertices[i].Pos += deltaPos;
				vertices[i].TexCoord += deltaTex;
				vertices[i].Color = Colors.FromBase255Vector4(vertices[i].Color.ToVector4() + deltaColor);
			}
		}

		/// <summary>
		/// Draws the vertices
		/// </summary>
		/// <param name="device"></param>
		/// <param name="vertices"></param>
		public void Draw(IDrawDevice device, VertexC1P3T2[] vertices)
		{
			device.AddVertices(Material, VertexMode.Quads, vertices);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RPG
{
	public class ScaledViewPanel : Panel
	{
		private Model World;
		private PointF centerLogicalPos;
		private bool dragInProgress;
		private Point dragStart;
		private PointF dragStartCenter;
		private PointF mouseLogicalPos;
		private float zoomScale;

		public ScaledViewPanel(Model model)
		{
			World = model;
			FitToWindow = true;
			zoomScale = 1f;
			centerLogicalPos = new PointF(-1, -1);
		}

        #region ScaledView
        public PointF MouseLogicalPos => mouseLogicalPos;

		public bool IsDraged;

		public PointF CenterLogicalPos
		{
			get { return centerLogicalPos; }
			set
			{
				centerLogicalPos = value;
				FitToWindow = false;
			}
		}

		public float ZoomScale
		{
			get { return zoomScale; }
			set
			{
				zoomScale = Math.Min(1000f, Math.Max(0.001f, value));
				FitToWindow = false;
			}
		}

		public bool FitToWindow { get; set; }

		protected override void InitLayout()
		{
			base.InitLayout();
			ResizeRedraw = true;
			DoubleBuffered = true;
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);
			if (e.Button == MouseButtons.Middle)
				FitToWindow = true;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Right)
			{
				dragInProgress = true;
				dragStart = e.Location;
				dragStartCenter = CenterLogicalPos;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			dragInProgress = false;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			mouseLogicalPos = ToLogical(e.Location);
			if (dragInProgress)
			{
				IsDraged = true;
				var loc = e.Location;
				var dx = (loc.X - dragStart.X) / ZoomScale;
				var dy = (loc.Y - dragStart.Y) / ZoomScale;
				CenterLogicalPos = new PointF(dragStartCenter.X - dx, dragStartCenter.Y - dy);
				Invalidate();
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			IsDraged = true;
			const float zoomChangeStep = 1.1f;
			if (e.Delta > 0)
				ZoomScale *= zoomChangeStep;
			if (e.Delta < 0)
				ZoomScale /= zoomChangeStep;
			Invalidate();
		}

		private PointF ToLogical(Point p)
		{
			var shift = GetShift();
			return new PointF(
				(p.X - shift.X) / zoomScale,
				(p.Y - shift.Y) / zoomScale);
		}

		private PointF GetShift()
		{
			return new PointF(
				ClientSize.Width / 2f - CenterLogicalPos.X * ZoomScale,
				ClientSize.Height / 2f - CenterLogicalPos.Y * ZoomScale);
		}
		#endregion

		protected override void OnPaint(PaintEventArgs e)
		{
			var brushForNotVisited = new SolidBrush(Color.Gray);
			var brushForVisited = new SolidBrush(Color.FromArgb(100,Color.Gray));
			var linePen = new Pen(Color.Black);
			var tileSize = 30;
			var whiteSpace = 30;
			var g = e.Graphics;
			base.OnPaint(e);
			g.Clear(Color.CornflowerBlue);
			var sceneSize = new SizeF(300,300);
			if (FitToWindow)
			{
				var vMargin = sceneSize.Width * ClientSize.Width < ClientSize.Height * sceneSize.Height;
				zoomScale = vMargin
					? ClientSize.Width / sceneSize.Height
					: ClientSize.Height / sceneSize.Height;
				centerLogicalPos = new PointF(350, 350);
				if (IsDraged)
					centerLogicalPos = new PointF(sceneSize.Width / 2, sceneSize.Height / 2);
			}
			var shift = GetShift();
			e.Graphics.ResetTransform();
			e.Graphics.TranslateTransform(shift.X, shift.Y);
			e.Graphics.ScaleTransform(ZoomScale, ZoomScale);

			for (int i = 0; i <= World.Map.Size; i++)
			{
				for (var j = 0; j <= World.Map.Size; j++)
				{
					if (i < World.Map.Size && j < World.Map.Size)
					{
						var tile = new Point(i, j);
						var rectangle = new Rectangle(i * tileSize + whiteSpace, j * tileSize + whiteSpace, tileSize, tileSize);
						g.DrawImage(World.Map[tile].Content.Background, rectangle);
						if (World.Map[tile].Content.Image != Properties.Resources.empty)
							g.DrawImage(World.Map[tile].Content.Image, rectangle);
						if (!World.Player.IsVisible(new Point(i, j)))
						{
							if (!World.Map[tile].IsVisited)
								g.FillRectangle(brushForNotVisited, rectangle);
							else
								g.FillRectangle(brushForVisited, rectangle);
						}
						else
							World.Map[tile].IsVisited = true;
					}
				}
			}
			for (int i = 0; i <= World.Map.Size; i++)
            {
				g.DrawLine(linePen, i * tileSize + whiteSpace, whiteSpace, i * tileSize + whiteSpace, World.Map.Size * tileSize + whiteSpace);
				g.DrawLine(linePen, whiteSpace, i * tileSize + whiteSpace, World.Map.Size * tileSize + whiteSpace, i * tileSize + whiteSpace);
			}
			var playerPos = World.Player.Position;
			g.DrawImage(World.Player.skin, new Rectangle(playerPos.X * tileSize + whiteSpace + 1, playerPos.Y * tileSize + whiteSpace + 1, tileSize - 1, tileSize - 1));
		}

	}
}
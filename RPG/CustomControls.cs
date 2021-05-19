using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPG
{
    class Bar : ProgressBar
    {
        public Brush Brush;
        public Bar()
        {
            SetStyle(ControlStyles.UserPaint, true);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            Rectangle rectangle = e.ClipRectangle;
            rectangle.Width = (int)(rectangle.Width * ((double)Value / Maximum));
            ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
            e.Graphics.FillRectangle(Brush, 0, 0, rectangle.Width, rectangle.Height);
        }
    }
}

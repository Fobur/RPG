using System;
using System.Drawing;
using System.Windows.Forms;

namespace RPG
{
    class Bar : ProgressBar
    {
        public Brush Brush;
        public Func<int> GetMaximum;
        public Func<int> GetValue;

        public Bar()
        {
            SetStyle(ControlStyles.UserPaint, true);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Maximum = GetMaximum();
            Value = GetValue();
            Rectangle rectangle = e.ClipRectangle;
            rectangle.Width = (int)(rectangle.Width * ((double)Value / Maximum));
            ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
            e.Graphics.FillRectangle(Brush, 0, 0, rectangle.Width, rectangle.Height);
        }
    }

    class LabelWithValue : Label
    {
        public Func<int> GetValue;
        public Stats Stat;

        protected override void OnPaint(PaintEventArgs e)
        {
            Text = Stat.ToString() + " : " + GetValue().ToString();
            base.OnPaint(e);
        }
    }

    class ButtonWithStat : Button
    {
        public Stats Stat;
    }
}

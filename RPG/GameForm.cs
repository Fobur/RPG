using System.Drawing;
using System.Windows.Forms;

namespace RPG
{
    public partial class GameForm : Form
    {
        public GameForm()
        {
            DoubleBuffered = true;
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var map = new Map(21);
            var linePen = new Pen(Color.Black);
            var tileSize = 30;
            var whiteSpace = 30;
            var g = e.Graphics;
            base.OnPaint(e);
            g.Clear(Color.CornflowerBlue);

            for (int i = 0; i <= map.Size; i++)
            {                  
                for (var j = 0; j <= map.Size; j++)
                {
                    if (i < map.Size && j < map.Size)
                        g.DrawImage(map.Grid[i].Content.Image, new Rectangle(i * tileSize + whiteSpace + 1, j * tileSize + whiteSpace + 1, tileSize - 1, tileSize - 1));
                    g.DrawLine(linePen, i * tileSize + whiteSpace, j + whiteSpace, i * tileSize + whiteSpace, j * tileSize + whiteSpace);
                    g.DrawLine(linePen, j + whiteSpace, i * tileSize + whiteSpace, j * tileSize + whiteSpace, i * tileSize + whiteSpace);
                }
            }
        }
    }
}

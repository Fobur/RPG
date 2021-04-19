using System.Drawing;
using System.Windows.Forms;

namespace RPG
{
    public partial class GameForm : Form
    {
        Model World = new Model();

        public GameForm()
        {
            DoubleBuffered = true;
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var linePen = new Pen(Color.Black);
            var tileSize = 30;
            var whiteSpace = 30;
            var g = e.Graphics;
            base.OnPaint(e);
            g.Clear(Color.CornflowerBlue);

            for (int i = 0; i <= World.Map.Size; i++)
            {
                for (var j = 0; j <= World.Map.Size; j++)
                {
                    if (i < World.Map.Size && j < World.Map.Size)
                        g.DrawImage(World.Map.Grid[i, j].Content.Image, new Rectangle(i * tileSize + whiteSpace + 1, j * tileSize + whiteSpace + 1, tileSize - 1, tileSize - 1));
                    g.DrawLine(linePen, i * tileSize + whiteSpace, j + whiteSpace, i * tileSize + whiteSpace, j * tileSize + whiteSpace);
                    g.DrawLine(linePen, j + whiteSpace, i * tileSize + whiteSpace, j * tileSize + whiteSpace, i * tileSize + whiteSpace);
                }
            }
            var playerPos = World.Player.Position;
            g.DrawImage(World.Player.skin, new Rectangle(playerPos.X * tileSize + whiteSpace + 1, playerPos.Y * tileSize + whiteSpace + 1, tileSize - 1, tileSize - 1));
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            World.Player.TakeStep(TranformKeyToDirection(e), World.Map);
            Refresh();
        }

        private MoveDirections TranformKeyToDirection(KeyEventArgs e)
        {
            var key = e.KeyCode;
            switch (key)
            {
                case Keys.Up:
                    return MoveDirections.Up;
                case Keys.Down:
                    return MoveDirections.Down;
                case Keys.Left:
                    return MoveDirections.Left;
                case Keys.Right:
                    return MoveDirections.Right;
                default:
                    return MoveDirections.None;
            }
        }
    }
}

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RPG
{
	public partial class GameForm : Form
	{
		Model World;
		ScaledViewPanel MainGameView;

		public GameForm()
		{
			KeyPreview = true;
			ClientSize = new Size(700, 700);
			DoubleBuffered = true;
			World = new Model();
			MainGameView = new ScaledViewPanel(World) { Dock = DockStyle.Fill };
			TakeStep.Click += TakeStepClicked;
            Resize += GameFormResized;
			Controls.Add(TakeStep);
			Controls.Add(MainGameView);
		}

        private void GameFormResized(object sender, System.EventArgs e)
        {
			TakeStep.Location = new Point(ClientSize.Width - 100, ClientSize.Height - 50);
        }

        private void TakeStepClicked(object sender, System.EventArgs e)
		{
			World.Player.RestoreEnergy();
		}

		private Button TakeStep = new Button
		{
			Text = "Take Move",
			Size = new Size(100, 50),
			Location = new Point(600, 650)
		};


		KeyEventArgs FirstPressed;
		HashSet<Keys> KeyPressed = new HashSet<Keys>();
		HashSet<Keys> ControlKeys = new HashSet<Keys>{Keys.W, Keys.A, Keys.S, Keys.D};

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (KeyPressed.Count == 0)
			{
				FirstPressed = e;
			}
			if (!KeyPressed.Contains(e.KeyCode))
				KeyPressed.Add(e.KeyCode);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			KeyPressed.Remove(e.KeyCode);
			if (KeyPressed.Count == 0 && ControlKeys.Contains(e.KeyCode))
			{
				World.Player.TakeStep(TranformKeyToDirection(FirstPressed), World.Map);
				Refresh();
			}
		}

		private MoveDirections TranformKeyToDirection(KeyEventArgs e)
		{
			var key = e.KeyCode;
			switch (key)
			{
				case Keys.W:
					return MoveDirections.Up;
				case Keys.S:
					return MoveDirections.Down;
				case Keys.A:
					return MoveDirections.Left;
				case Keys.D:
					return MoveDirections.Right;
				default:
					return MoveDirections.None;
			}
		}
    }
}

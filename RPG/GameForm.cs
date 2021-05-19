using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
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
			ClientSize = new Size(1050, 700);
			DoubleBuffered = true;
			World = new Model();
			MainGameView = new ScaledViewPanel(World) { Dock = DockStyle.Fill };

			PrivateFontCollection collection = new PrivateFontCollection();
			collection.AddFontFile(@"E:\Game\RPG\RPG\Resources\Konstanting.ttf");
			FontFamily fontFamily = new FontFamily("Konstanting", collection);
			RestoreHP.Font = new Font(fontFamily, 18);
			TakeStep.Font = new Font(fontFamily, 20);
			HP.Font = new Font(fontFamily, 20);
			Energy.Font = new Font(fontFamily, 20);

			TakeStep.Click += TakeStepClicked;
			EnergyBar.Value = World.Player.Energy;
			EnergyBar.Maximum = World.Player.MaxEnergy;
			EnergyBar.Controls.Add(Energy);
			RestoreHP.Click += RestoreHPClicked;
			HpBar.Value = World.Player.HP;
			HpBar.Maximum = World.Player.MaxHP;
			HpBar.Controls.Add(HP);

			MapView.Controls.Add(MainGameView);

			Interface.Controls.Add(TakeStep);
			Interface.Controls.Add(EnergyBar);
			Interface.Controls.Add(HpBar);
			Interface.Controls.Add(RestoreHP);

			Controls.Add(Interface);
			Controls.Add(MapView);

			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
		}

		private Label HP = new Label
		{
			Text = "HP",
			Size = new Size(Interface.Size.Width - 50, 50),
			TextAlign = ContentAlignment.MiddleCenter,
			BackColor = Color.Transparent,
			ForeColor = Color.Black
		};

		private Bar HpBar = new Bar
		{
			Location = new Point(25,25),
			Size = new Size(Interface.Size.Width - 50, 50),
			Brush = Brushes.ForestGreen
		};

		private void RestoreHPClicked(object sender, System.EventArgs e)
        {
			World.Player.RestoreHP();
        }

		private Button RestoreHP = new Button
		{
			Text = "Restore HP",
			Size = new Size(100, 50),
			Location = new Point(Interface.Size.Width - 125, Interface.Size.Height - 55),
			BackgroundImage = Properties.Resources.stone,
			FlatStyle = FlatStyle.Popup
		};

		private Label Energy = new Label
		{
			Text = "Energy",
			Size = new Size(Interface.Size.Width - 50, 50),
			TextAlign = ContentAlignment.MiddleCenter,
			BackColor = Color.Transparent,
			ForeColor = Color.Black
		};

		private Bar EnergyBar = new Bar
		{
			Location = new Point(25, 85),
			Size = new Size(Interface.Size.Width - 50, 50),
			Brush = Brushes.CornflowerBlue
		};

		private void TakeStepClicked(object sender, System.EventArgs e)
		{
			World.Player.RestoreEnergy();
		}

		private Button TakeStep = new Button
		{
			Text = "Take Move",
			Size = new Size(100, 50),
			Location = new Point(25, Interface.Size.Height - 55),
			BackgroundImage = Properties.Resources.stone,
			FlatStyle = FlatStyle.Popup
		};

		private static ContainerControl Interface = new ContainerControl
		{
			Bounds = new Rectangle(new Point(700, 0), new Size(350, 700)),
			BackgroundImage = Properties.Resources.wood
		};
		
		private ContainerControl MapView = new ContainerControl
		{
			Bounds = new Rectangle(new Point(0,0),new Size(700,700))
		};

		KeyEventArgs FirstPressed;
		HashSet<Keys> KeyPressed = new HashSet<Keys>();
		HashSet<Keys> ControlKeys = new HashSet<Keys> { Keys.W, Keys.A, Keys.S, Keys.D };

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

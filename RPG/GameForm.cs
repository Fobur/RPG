using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace RPG
{
	public partial class GameForm : Form
	{
		Model World;
		ScaledViewPanel MainGameView;
		Timer GameTimer = new Timer();
		private static int currentStep;

		public GameForm(FontFamily fontFamily)
		{
			KeyPreview = true;
			ClientSize = new Size(1050, 700);
			DoubleBuffered = true;
			World = new Model();
			Controls.Add(Log);
			Log.Columns.Add("Step", 35);
			Log.Columns.Add("Message", 645);
			World.Map.GetGameForm = () => this;
			MainGameView = new ScaledViewPanel(World) { Dock = DockStyle.Fill };
			MainGameView.MapSize = World.Map.Size;
			MapView.Controls.Add(MainGameView);

			#region Fonts
			
			RestoreHP.Font = new Font(fontFamily, 18);
			TakeMove.Font = new Font(fontFamily, 20);
			HP.Font = new Font(fontFamily, 20);
			Energy.Font = new Font(fontFamily, 20);
			Exp.Font = new Font(fontFamily, 20);
			Level.Font = new Font(fontFamily, 20);
			Agility.Font = new Font(fontFamily, 20);
			Stamina.Font = new Font(fontFamily, 20);
			Perception.Font = new Font(fontFamily, 20);
			Strength.Font = new Font(fontFamily, 20);
			StatPoints.Font = new Font(fontFamily, 20);
			RoundAttack.Font = new Font(fontFamily, 19);
			RangeAttack.Font = new Font(fontFamily, 19);
			HeavyAttack.Font = new Font(fontFamily, 19);
			Treasures.Font = new Font(fontFamily, 22);
			LogButton.Font = new Font(fontFamily, 19);
			#endregion

			AddIntoLog("Game started", Color.Black);

			#region ControlsActivities
			TakeMove.Click += TakeMoveClicked;
			EnergyBar.GetMaximum = () => World.Player.MaxEnergy;
			EnergyBar.GetValue = () => World.Player.Energy;
			EnergyBar.Controls.Add(Energy);
			RestoreHP.Click += RestoreHPClicked;
			HpBar.GetMaximum = () => World.Player.MaxHP;
			HpBar.GetValue = () => World.Player.HP;
			HpBar.Controls.Add(HP);
			ExpBar.GetMaximum = () => World.Player.Level * 10;
			ExpBar.GetValue = () => World.Player.Experience;
			ExpBar.Controls.Add(Exp);
			Level.GetValue = () => World.Player.Level;
			Agility.GetValue = () => World.Player.Stats.Agility;
			Strength.GetValue = () => World.Player.Stats.Strength;
			Stamina.GetValue = () => World.Player.Stats.Stamina;
			Perception.GetValue = () => World.Player.Stats.Perception;
			StatPoints.GetValue = () => World.Player.StatPoints;
			IncreaseAgility.Click += IncreaseStatClicked;
			IncreasePerception.Click += IncreaseStatClicked;
			IncreaseStamina.Click += IncreaseStatClicked;
			IncreaseStrength.Click += IncreaseStatClicked;
			RoundAttack.Click += AttackClicked;
			RangeAttack.Click += AttackClicked;
			HeavyAttack.Click += AttackClicked;
			LogButton.Click += LogClicked;
			#endregion

			#region StatInterface
			StatInterface.Paint += StatInterface_Paint;
			StatInterface.Controls.Add(Level);
			StatInterface.Controls.Add(Agility);
			StatInterface.Controls.Add(IncreaseAgility);
			StatInterface.Controls.Add(Strength);
			StatInterface.Controls.Add(IncreaseStrength);
			StatInterface.Controls.Add(Stamina);
			StatInterface.Controls.Add(IncreaseStamina);
			StatInterface.Controls.Add(Perception);
			StatInterface.Controls.Add(IncreasePerception);
			StatInterface.Controls.Add(StatPoints);
			StatInterface.Controls.Add(Treasures);
			#endregion

			#region MainInterface
			Interface.Controls.Add(RangeAttack);
			Interface.Controls.Add(RoundAttack);
			Interface.Controls.Add(HeavyAttack);
			Interface.Controls.Add(StatInterface);
			Interface.Controls.Add(TakeMove);
			Interface.Controls.Add(EnergyBar);
			Interface.Controls.Add(HpBar);
			Interface.Controls.Add(ExpBar);
			Interface.Controls.Add(RestoreHP);
			Interface.Controls.Add(LogButton);
			#endregion

			Controls.Add(Interface);
			Controls.Add(MapView);

			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;

			GameTimer.Interval = 700;
			GameTimer.Tick += GameTimerTicked;
			GameTimer.Start();
		}

		private void StatInterface_Paint(object sender, PaintEventArgs e)
		{
			var rectangle = new Rectangle(new Point(230, 50), new Size(80, 70));
			foreach (var treasure in World.Player.Treasures)
			{
				e.Graphics.DrawImage(treasure.Skin, rectangle);
				rectangle.Location = new Point(rectangle.Location.X, rectangle.Location.Y + 80);
			};
		}

		private void GameTimerTicked(object sender, EventArgs e)
		{
			if (World.Player.StatPoints > 0)
			{
				IncreaseAgility.Visible = true;
				IncreaseStrength.Visible = true;
				IncreaseStamina.Visible = true;
				IncreasePerception.Visible = true;
				Interface.Refresh();
			}
		}

		#region Interface

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			CheckStatPoints(World);
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
			Location = new Point(25, 25),
			Size = new Size(Interface.Size.Width - 50, 50),
			Brush = Brushes.ForestGreen,
			Name = "HPBar"
		};

		private void RestoreHPClicked(object sender, EventArgs e)
		{
			World.Player.RestoreHP();
			AddIntoLog("HP Restored", Color.Green);
			Refresh();
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
			Brush = Brushes.CornflowerBlue,
			Name = "EnergyBar"
		};

		private Label Exp = new Label
		{
			Text = "Experience",
			Size = new Size(Interface.Size.Width - 50, 50),
			TextAlign = ContentAlignment.MiddleCenter,
			BackColor = Color.Transparent,
			ForeColor = Color.Black
		};
		private Bar ExpBar = new Bar
		{
			Location = new Point(25, 145),
			Size = new Size(Interface.Size.Width - 50, 50),
			Brush = Brushes.LightGoldenrodYellow,
			Name = "ExpBar"
		};

		private void TakeMoveClicked(object sender, EventArgs e)
		{
			World.Player.RestoreEnergy();
			foreach (var monster in World.Monsters)
				monster.TakeMove();
			World.CheckMonsters();
			if (World.Player.IsDead)
			{
				IncreaseAgility.Visible = false;
				IncreasePerception.Visible = false;
				IncreaseStamina.Visible = false;
				IncreaseStrength.Visible = false;
				RestoreHP.Visible = false;
				HeavyAttack.Visible = false;
				RangeAttack.Visible = false;
				RoundAttack.Visible = false;
				TakeMove.Visible = false;
			}
			currentStep++;
			AddIntoLog("Energy Restored", Color.Green);
			Refresh();
		}
		private Button TakeMove = new Button
		{
			Text = "Take Move",
			Size = new Size(100, 50),
			Location = new Point(25, Interface.Size.Height - 55),
			BackgroundImage = Properties.Resources.stone,
			FlatStyle = FlatStyle.Popup
		};

		private void LogClicked(object sender, EventArgs e)
		{
			Log.Visible = !Log.Visible;
		}
		private Button LogButton = new Button
		{
			Text = "Log",
			Size = new Size(70, 50),
			Location = new Point(140, Interface.Size.Height - 55),
			BackgroundImage = Properties.Resources.stone,
			FlatStyle = FlatStyle.Popup
		};

		private static ContainerControl Interface = new ContainerControl
		{
			Bounds = new Rectangle(new Point(700, 0), new Size(350, 700)),
			BackgroundImage = Properties.Resources.wood
		};
		private static ContainerControl StatInterface = new ContainerControl
		{
			Bounds = new Rectangle(new Point(10, 205), new Size(330, 300)),
			BackgroundImage = Properties.Resources.stone
		};

		private Label Treasures = new Label
		{
			Text = "Treasures",
			Size = new Size(150, 50),
			Location = new Point(230, 10),
			BackColor = Color.Transparent
		};
		private LabelWithValue StatPoints = new LabelWithValue
		{
			Stat = Stats.StatPoints,
			Size = new Size(150, 50),
			Location = new Point(10, 260),
			BackColor = Color.Transparent,
			ForeColor = Color.Black
		};
		private LabelWithValue Level = new LabelWithValue
		{
			Stat = Stats.Level,
			Size = new Size(150, 50),
			Location = new Point(10, 10),
			BackColor = Color.Transparent,
			ForeColor = Color.Black
		};
		private LabelWithValue Agility = new LabelWithValue
		{
			Stat = Stats.Agility,
			Size = new Size(150, 50),
			Location = new Point(10, 60),
			BackColor = Color.Transparent,
			ForeColor = Color.Black
		};
		private LabelWithValue Strength = new LabelWithValue
		{
			Stat = Stats.Strength,
			Size = new Size(150, 50),
			Location = new Point(10, 110),
			BackColor = Color.Transparent,
			ForeColor = Color.Black
		};
		private LabelWithValue Stamina = new LabelWithValue
		{
			Stat = Stats.Stamina,
			Size = new Size(150, 50),
			Location = new Point(10, 160),
			BackColor = Color.Transparent,
			ForeColor = Color.Black
		};
		private LabelWithValue Perception = new LabelWithValue
		{
			Stat = Stats.Perception,
			Size = new Size(150, 50),
			Location = new Point(10, 210),
			BackColor = Color.Transparent,
			ForeColor = Color.Black
		};

		private static ButtonWithStat IncreaseAgility = new ButtonWithStat
		{
			Size = new Size(31, 31),
			Location = new Point(160, 60),
			BackgroundImage = Properties.Resources.plus,
			FlatStyle = FlatStyle.Popup,
			Stat = Stats.Agility
		};
		private static ButtonWithStat IncreaseStrength = new ButtonWithStat
		{
			Size = new Size(31, 31),
			Location = new Point(160, 110),
			BackgroundImage = Properties.Resources.plus,
			FlatStyle = FlatStyle.Popup,
			Stat = Stats.Strength
		};
		private static ButtonWithStat IncreaseStamina = new ButtonWithStat
		{
			Size = new Size(31, 31),
			Location = new Point(160, 160),
			BackgroundImage = Properties.Resources.plus,
			FlatStyle = FlatStyle.Popup,
			Stat = Stats.Stamina
		};
		private static ButtonWithStat IncreasePerception = new ButtonWithStat
		{
			Size = new Size(31, 31),
			Location = new Point(160, 210),
			BackgroundImage = Properties.Resources.plus,
			FlatStyle = FlatStyle.Popup,
			Stat = Stats.Perception
		};

		private static Button RoundAttack = new Button
		{
			Text = "Round Attack",
			Size = new Size(75, 75),
			Location = new Point(25, Interface.Size.Height - 165),
			FlatStyle = FlatStyle.Popup,
			BackgroundImage = Properties.Resources.stone,
			Name = "Round"
		};
		private static Button RangeAttack = new Button
		{
			Text = "Range Attack",
			Size = new Size(75, 75),
			Location = new Point(135, Interface.Size.Height - 165),
			FlatStyle = FlatStyle.Popup,
			BackgroundImage = Properties.Resources.stone,
			Name = "Range"
		};
		private static Button HeavyAttack = new Button
		{
			Text = "Heavy Attack",
			Size = new Size(75, 75),
			Location = new Point(245, Interface.Size.Height - 165),
			FlatStyle = FlatStyle.Popup,
			BackgroundImage = Properties.Resources.stone,
			Name = "Heavy"
		};

		private void AttackClicked(object sender, EventArgs e)
		{
			var button = (Button)sender;
			Attack attack = AttackMethods.AttackConstructor(AttackMethods.ConvertFromString(button.Name),
				World.Player);
			if (World.Player.Energy >= attack.Cost)
			{
				World.Player.Energy -= attack.Cost;
				MainGameView.AttackZone = attack.Zone;
				MainGameView.IsAttacked = true;
				Refresh();
				World.CheckAttackZone(attack, true, World.Player);
				MainGameView.IsAttacked = false;
				Refresh();
			}
			AddIntoLog("Not enough energy for attack", Color.Orange);
		}

		private void IncreaseStatClicked(object sender, System.EventArgs e)
		{
			var a = (ButtonWithStat)sender;
			World.Player.IncreaseStat(a.Stat, 1);
			CheckStatPoints(World);
			Refresh();
		}

		public static void CheckStatPoints(Model World)
		{
			if (World.Player.StatPoints > 0)
			{
				IncreaseAgility.Visible = true;
				IncreaseStrength.Visible = true;
				IncreaseStamina.Visible = true;
				IncreasePerception.Visible = true;
			}
			else
			{
				IncreaseAgility.Visible = false;
				IncreaseStrength.Visible = false;
				IncreaseStamina.Visible = false;
				IncreasePerception.Visible = false;
			}
		}
		#endregion

		private static ListView Log = new ListView
		{
			BackColor = Color.LightSlateGray,
			Size = new Size(700, 150),
			Location = new Point(0, 550),
			BorderStyle = BorderStyle.FixedSingle,
			Scrollable = true,
			View = View.Details,
			FullRowSelect = true,
			Alignment = ListViewAlignment.SnapToGrid,
			Visible = false,
			AutoScrollOffset = new Point(690, 700)
		};
		public static void AddIntoLog(string msg, Color color)
		{
			var log = new string[] { currentStep.ToString(), msg };
			Log.Items.Add(new ListViewItem(log, 0, color, Log.BackColor, new Font("Arial", 9)));
			Log.Items[Log.Items.Count - 1].EnsureVisible();
		}

		private ContainerControl MapView = new ContainerControl
		{
			Bounds = new Rectangle(new Point(0, 0), new Size(700, 700))
		};

		KeyEventArgs FirstPressed;
		HashSet<Keys> KeyPressed = new HashSet<Keys>();
		HashSet<Keys> ControlKeys = new HashSet<Keys> { Keys.W, Keys.A, Keys.S, Keys.D };

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (!World.Player.IsDead)
			{
				if (KeyPressed.Count == 0)
				{
					FirstPressed = e;
				}
				if (!KeyPressed.Contains(e.KeyCode))
					KeyPressed.Add(e.KeyCode);
			}
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (!World.Player.IsDead)
			{
				KeyPressed.Remove(e.KeyCode);
				if (KeyPressed.Count == 0 && ControlKeys.Contains(e.KeyCode) && World.Map.CanTakeMove(TranformKeyToDirection(FirstPressed), World.Player))
				{
					World.Map.TakeMove(TranformKeyToDirection(FirstPressed), World.Player);
					RestoreHP.Visible = World.Player.Position == World.Map.Hub ? true : false;
					Refresh();
				}
				else if (KeyPressed.Count == 0 && ControlKeys.Contains(e.KeyCode) &&World.Map[World.Map.DirectionToPoint(TranformKeyToDirection(FirstPressed), World.Player.Position)].Content.Cost > World.Player.Energy)
					AddIntoLog("Not enough energy to move in this cell", Color.OrangeRed);
			}
		}
		private Directions TranformKeyToDirection(KeyEventArgs e)
		{
			var key = e.KeyCode;
			switch (key)
			{
				case Keys.W:
					return Directions.Up;
				case Keys.S:
					return Directions.Down;
				case Keys.A:
					return Directions.Left;
				case Keys.D:
					return Directions.Right;
				default:
					return Directions.None;
			}
		}
	}
}
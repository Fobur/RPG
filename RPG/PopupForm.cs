using System.Drawing;
using System.Windows.Forms;

namespace RPG
{
    public partial class PopupForm : Form
    {
        public PopupForm(Label label)
        {
            label.TextAlign = ContentAlignment.MiddleCenter;
            Size = new Size(100, 50);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Controls.Add(label);
            InitializeComponent();
        }
    }
}

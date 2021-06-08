using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace RPG
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var collection = new PrivateFontCollection();
            var temp = Environment.CurrentDirectory.Split('\\').ToList();
            temp.Reverse();
            var path = temp.SkipWhile(x => x != "RPG").ToList();
            path.Reverse();
            var directory = string.Join('\\', path);
            collection.AddFontFile(directory + @"\Resources\Konstanting.ttf");
            var fontFamily = new FontFamily("Konstanting", collection);
            Application.Run(new GameForm(fontFamily));
        }
    }
}
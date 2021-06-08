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
            PrivateFontCollection collection = new PrivateFontCollection();
            var temp = Environment.CurrentDirectory.Split('\\')
                .TakeWhile(x => x != "RPG")
                .ToArray();
            var directory = string.Join('\\', temp);
            collection.AddFontFile(directory + @"\RPG\RPG\Resources\Konstanting.ttf");
            FontFamily fontFamily = new FontFamily("Konstanting", collection);
            Application.Run(new GameForm(fontFamily));
        }
    }
}
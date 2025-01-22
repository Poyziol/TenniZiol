using System;
using System.Windows.Forms;
using src;

namespace Tennis
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenu());  // Lancer le menu principal
        }
    }
}

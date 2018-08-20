using System.Windows.Forms;
using GameManager;

namespace Program
{
    public class Program
    {
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormSettings formSettingsForDamka = new FormSettings();
            formSettingsForDamka.ShowDialog();
        }
    }
}
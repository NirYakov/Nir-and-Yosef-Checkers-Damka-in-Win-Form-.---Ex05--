using System.Windows.Forms;
using LogicPart;

namespace BoardPart
{
    internal class ButtonLocat : Button
    {
        private Locat m_LocatOfButton;

        public ButtonLocat(Locat i_LocateToButton)
        {
            m_LocatOfButton = i_LocateToButton;
        }

        public Locat LocatOfButton
        {
            get
            {
                return m_LocatOfButton;
            }
        }
    }
}
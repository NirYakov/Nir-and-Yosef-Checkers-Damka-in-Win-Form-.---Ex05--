namespace GameManager
{
    internal struct Player
    {
        private string m_Name;
        private short m_Points;

        public Player(string i_NameOfPlayer = "Unknown")
        {
            m_Name = i_NameOfPlayer;
            m_Points = 0;
        }

        public string Name
        {
            get
            {
                return m_Name;
            }

            set
            {
                m_Name = value;
            }
        }

        public short Points
        {
            get
            {
                return m_Points;
            }

            set
            {
                m_Points = value;
            }
        }
    }
}
namespace LogicPart
{
    public struct Locat
    {
        private byte m_X;
        private byte m_Y;

        public Locat(byte x, byte y)
        {
            m_X = x;
            m_Y = y;
        }

        public byte X
        {
            get
            {
                return m_X;
            }

            set
            {
                m_X = value;
            }
        }

        public byte Y
        {
            get
            {
                return m_Y;
            }

            set
            {
                m_Y = value;
            }
        }
    }
}

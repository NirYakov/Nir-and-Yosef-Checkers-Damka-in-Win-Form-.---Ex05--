namespace LogicPart
{
    public class MoveToDo
    {
        private Locat m_Source;
        private Locat m_Dest;

        public MoveToDo(Locat i_Source, Locat i_Dest)
        {
            m_Source = i_Source;
            m_Dest = i_Dest;
        }

        public Locat Source
        {
            get
            {
                return m_Source;
            }

            set
            {
                m_Source = value;
            }
        }

        public Locat Dest
        {
            get
            {
                return m_Dest;
            }

            set
            {
                m_Dest = value;
            }
        }
    }
}
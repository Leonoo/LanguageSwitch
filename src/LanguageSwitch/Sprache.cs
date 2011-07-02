using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LanguageSwitch
{
    public class Language
    {
        private string m_Name;

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private string m_Culture;

        public string Culture
        {
            get { return m_Culture; }
            set { m_Culture = value; }
        }

        private List<String> m_Ids = new List<string>();

        public List<String> Ids
        {
            get { return m_Ids; }
            set { m_Ids = value; }
        }
    }
}

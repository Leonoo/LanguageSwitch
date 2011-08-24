using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;

namespace LanguageSwitch
{
    public class WPFTranslate : INotifyPropertyChanged
    {
        public WPFTranslate()
        {
            Translate.TanslateList.Add(this);
        }

        ~WPFTranslate()
        {
            if (Translate.TanslateList != null && Translate.TanslateList.Contains(this))
            {
                Translate.TanslateList.Remove(this);
            }
        }

        public Dictionary<String, String> Translation
        {
            get
            {
                return Translate.Translation;
            }

            set
            {
                OnPropertyChanged("Translation");
            }
        }

        public CultureInfo Culture
        {
            get
            {
                return Translate.Culture;
            }

            set
            {
                OnPropertyChanged("Culture");
                OnPropertyChanged("XMLLanguage");
            }
        }

        public XmlLanguage XMLLanguage
        {
            get
            {
                return XmlLanguage.GetLanguage(Culture.Name);
            }
            set
            {
                OnPropertyChanged("XMLLanguage");
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion // INotifyPropertyChanged Members
    }

    public static class Translate
    {
        public static List<WPFTranslate> TanslateList = new List<WPFTranslate>();

        private static bool m_First = true;

        private static Language m_DefaultLanguage = new Language { Culture = "en-GB", Name = "English (Great Britain)", Ids = new List<string> { "en", "en-GB" } };

        public static Language DefaultLanguage
        {
            get { return m_DefaultLanguage; }
            set { m_DefaultLanguage = value; }
        }

        private static Language m_CurrentLanguage;

        public static Language CurrentLanguage
        {
            get { return m_CurrentLanguage; }
            private set { m_CurrentLanguage = value; }
        }

        private static CultureInfo m_Culture = System.Globalization.CultureInfo.CurrentCulture;

        public static CultureInfo Culture
        {
            get { return m_Culture; }
            set
            {
                if (m_Culture != value)
                {
                    m_Culture = value;
                    foreach (WPFTranslate item in TanslateList)
                    {
                        item.Culture = null;
                    }
                }
            }
        }

        private static List<Language> m_Languages;

        public static List<Language> Languages
        {
            get { return m_Languages; }
            set { m_Languages = value; }
        }

        private static Dictionary<String, String> m_Translation;

        public static Dictionary<String, String> Translation
        {
            get { return m_Translation; }
            set
            {
                m_Translation = value;
                //if (m_First)
                //{
                    foreach (WPFTranslate item in TanslateList)
                    {
                        item.Translation = null;
                    }
                //    m_First = false;
                //}
            }
        }

        private static string LoadValue(XmlNode itemNode, Language Language)
        {
            string value = null;

            foreach (string language in Language.Ids)
            {
                foreach (XmlNode item in itemNode.ChildNodes)
                {
                    if (language == item.Name)
                    {
                        value = item.InnerText;
                    }
                }
            }

            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("!NL!", Environment.NewLine);
            }

            return value;
        }

        public static void LoadLanguages(Stream stream)
        {
            XmlDocument xmlDoc = new XmlDocument();

            List<Language> tempLanguagen = new List<Language>();

            try
            {
                xmlDoc.Load(stream);

                foreach (XmlNode root in xmlDoc.SelectNodes("Languages"))
                {
                    foreach (XmlNode LanguageNode in root.SelectNodes("Language"))
                    {
                        Language tempLanguage = new Language();
                        tempLanguage.Name = LanguageNode.Attributes["Name"].Value;
                        tempLanguage.Culture = LanguageNode.Attributes["Culture"].Value;

                        foreach (XmlNode idNode in LanguageNode.SelectNodes("Id"))
                        {
                            tempLanguage.Ids.Add(idNode.Attributes["Name"].Value);
                        }

                        tempLanguagen.Add(tempLanguage);
                    }
                }

                Languages = tempLanguagen;
            }
            catch (Exception ex)
            {
                throw new Exception("Error - " + "SourceFile" + ": " + ex.Message);
            }
        }

        public static void Translated(Language language, Stream stream)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string Value = string.Empty;
            string Key = string.Empty;

            Dictionary<string, string> translation = new Dictionary<string, string>();

            try
            {
                xmlDoc.Load(stream);

                foreach (XmlNode root in xmlDoc.SelectNodes("Translate"))
                {
                    foreach (XmlNode itemNode in root.SelectNodes("Item"))
                    {
                        Value = LoadValue(itemNode, language);

                        if (string.IsNullOrEmpty(Value))
                        {
                            Value = LoadValue(itemNode, DefaultLanguage);
                        }

                        Key = itemNode.Attributes["Id"].Value;

                        if (!translation.ContainsKey(Key))
                        {
                            translation.Add(Key, Value);
                        }
                    }
                }

                Translation = translation;
                CurrentLanguage = language;
                Culture = new CultureInfo(language.Culture);
            }
            catch (Exception ex)
            {
                throw new Exception("Error - " + "SourceFile" + ": " + ex.Message);
            }
        }
    }
}

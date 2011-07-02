using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sample
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LanguageSwitch.WPFTranslate m_WPFTranslate = new LanguageSwitch.WPFTranslate();
        public LanguageSwitch.WPFTranslate WPFTranslate { get { return m_WPFTranslate; } }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            LanguageSwitch.View.Switch lSwitch = new LanguageSwitch.View.Switch();
            lSwitch.DataContext = LanguageSwitch.Translate.Languages;

            bool? result = lSwitch.ShowDialog();

            if (result.HasValue && result.Value)
            {
                using (FileStream fs = new FileStream(@".\Language.xml", FileMode.Open, FileAccess.Read))
                {
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lSwitch.SelectedLanguage.Culture);
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lSwitch.SelectedLanguage.Culture);
                    LanguageSwitch.Translate.Translated(lSwitch.SelectedLanguage, fs);
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(WPFTranslate.Translation["Sprache"]);
        }

        private void Sample_Loaded(object sender, RoutedEventArgs e)
        {
            using (FileStream fs = new FileStream(@".\Languages.xml", FileMode.Open, FileAccess.Read))
            {
                LanguageSwitch.Translate.LoadLanguages(fs);
            }

            using (FileStream fs = new FileStream(@".\Language.xml", FileMode.Open, FileAccess.Read))
            {
                LanguageSwitch.Translate.Translated(LanguageSwitch.Translate.Languages[0], fs);
            }
        }
    }
}

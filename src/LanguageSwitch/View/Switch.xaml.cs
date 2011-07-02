using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace LanguageSwitch.View
{
    /// <summary>
    /// Interaktionslogik für Switch.xaml
    /// </summary>
    public partial class Switch : Window
    {
        private WPFTranslate m_WPFTranslate = new WPFTranslate();
        public WPFTranslate WPFTranslate { get { return m_WPFTranslate; } }

        public Language SelectedLanguage { get; set; }

        public Switch()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Language> lala = DataContext as List<Language>;
            foreach (var item in lala)
            {
                if (item == Translate.CurrentLanguage)
                {
                    comboBox1.SelectedItem = item;
                    break;
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox1.SelectedItem is Language)
            {
                Language item = (Language)comboBox1.SelectedItem;

                SelectedLanguage = item;
                DialogResult = true;
            }
            else
            {
                DialogResult = false;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

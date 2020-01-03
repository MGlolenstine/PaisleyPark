using PaisleyPark.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PaisleyPark.Views
{
    /// <summary>
    /// Interaction logic for NewHotkey.xaml
    /// </summary>
    public partial class NewHotkey : Window
    {
        public NewHotkey()
        {
            InitializeComponent();
            KeysConverter kc = new KeysConverter();
            foreach (Keys k in Enum.GetValues(typeof(Keys)))
            {
                hotkeySelection.Items.Add(kc.ConvertToString(k));
            }
            hotkeySelection.SelectedIndex = 0;
            foreach (Preset p in Settings.Load().Presets)
            {
                presetSelection.Items.Add(p.Name);
            }
        }
    }
}

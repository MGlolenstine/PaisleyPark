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
    /// Interaction logic for EditHotkey.xaml
    /// </summary>
    public partial class EditHotkey : Window
    {
        private readonly static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public EditHotkey(Preset Preset, Keys Hotkey)
        {
            InitializeComponent();
            KeysConverter kc = new KeysConverter();
            foreach (Keys k in Enum.GetValues(typeof(Keys)))
            {
                hotkeySelection.Items.Add(kc.ConvertToString(k));
                if (k == Hotkey)
                {
                    hotkeySelection.SelectedIndex = hotkeySelection.Items.Count-1;
                }
            }
            var Presets = Settings.Load().Presets;
            foreach (Preset p in Presets)
            {
                presetSelection.Items.Add(p.Name);
                if (p.Name.Equals(Preset.Name))
                {
                    logger.Info("Found the preset at " + (presetSelection.Items.Count - 1));
                    presetSelection.SelectedIndex = presetSelection.Items.Count - 1;
                }
            }
        }
    }
}

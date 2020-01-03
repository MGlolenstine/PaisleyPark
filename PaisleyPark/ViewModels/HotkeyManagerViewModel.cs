using Newtonsoft.Json;
using PaisleyPark.Common;
using PaisleyPark.Models;
using PaisleyPark.Views;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PaisleyPark.ViewModels
{
    class HotkeyManagerViewModel
    {
		public Models.Hotkey SelectedItem { get; set; }
		public ObservableCollection<Models.Hotkey> Hotkeys { get; set; }
		public ICommand AddCommand { get; private set; }
		public ICommand RemoveCommand { get; private set; }
		public ICommand OKCommand { get; private set; }
		public ICommand EditCommand { get; private set; }
		public ICommand ImportCommand { get; private set; }
		public ICommand ExportCommand { get; private set; }

		public bool DialogResult { get; private set; }
		private readonly static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public HotkeyManagerViewModel(IEventAggregator ea)
		{
			// Create our commands.
			AddCommand = new DelegateCommand(OnAddHotkey);
			RemoveCommand = new DelegateCommand(OnRemoveHotkey);
			OKCommand = new DelegateCommand<Window>(OnOK);
			EditCommand = new DelegateCommand(OnEdit);
			ImportCommand = new DelegateCommand(OnImport);
			ExportCommand = new DelegateCommand(OnExport);
			Hotkeys = Settings.Load().Hotkeys;
		}

		/// <summary>
		/// Used to check if you can do a certain function on this view.
		/// </summary>
		/// <returns></returns>
		private bool CheckCanDo()
		{
			if (Application.Current.MainWindow == null || !Application.Current.MainWindow.IsInitialized)
			{
				MessageBox.Show("You can't create or modify a hotkey right now.", "Paisley Park", MessageBoxButton.OK, MessageBoxImage.Warning);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Adding new hotkey.
		/// </summary>
		private void OnAddHotkey()
		{
			// Can't do unless game is loaded.
			//if (!CheckCanDo())
			//	return;

			try
			{
				// Create window for add hotkey.
				var win = new NewHotkey
				{
					// Owner set to MainWindow.
					Owner = Application.Current.MainWindow
				};

				// Get the VM for this window.
				var vm = win.DataContext as NewHotkeyViewModel;

				// Show the dialog for the hotkey window.
				if (win.ShowDialog() == true)
				{
					// Initialize the creation of our hotkey with the hotkey name.
					var p = new Models.Hotkey() { Name = vm.Name, preset = GetPresetByName(vm.Preset), Key = (System.Windows.Forms.Keys)Enum.GetValues(typeof(System.Windows.Forms.Keys)).GetValue(vm.Hotkey) };
				   // Add the hotkey.
				   Hotkeys.Add(p);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something happened while creating your hotkey!", "Paisley Park", MessageBoxButton.OK, MessageBoxImage.Error);
				logger.Error(ex, "Exception while adding a new hotkey.");
			}
		}

		private Preset GetPresetByName(String name) 
		{
			foreach (Preset p in Settings.Load().Presets)
			{
				if (p.Name.Equals(name)) {
					return p;
				}
			}
			return null;
		}

		/// <summary>
		/// Removing selected hotkey.
		/// </summary>
		private void OnRemoveHotkey()
		{
			if (SelectedItem != null)
			{
				if (MessageBox.Show(
					"Are you sure you want to delete this hotkey?",
					"Paisley Park",
					MessageBoxButton.YesNo,
					MessageBoxImage.Warning) == MessageBoxResult.Yes) {
					Hotkeys.Remove(SelectedItem);
					//RemoveHotkey();
				}
			}
		}

		/// <summary>
		/// When editing the selected hotkey.
		/// </summary>
		private void OnEdit()
		{
			// Can't do unless game is loaded.
			//if (!CheckCanDo())
			//	return;

			try
			{
				if (SelectedItem == null)
					return;

				// Create Edit hotkey window.
				var win = new EditHotkey(SelectedItem.preset, SelectedItem.Key)
				{
					// Owner set to MainWindow.
					Owner = Application.Current.MainWindow
				};

				// Get the view model.
				var vm = win.DataContext as EditHotkeyViewModel;

				// Set the name to the selected name in the hotkey list.
				vm.Name = SelectedItem.Name;
				//vm.Preset_Object = SelectedItem.preset;
				//vm.Preset = SelectedItem.preset.Name;
				//vm.Hotkey = SelectedItem.Key;

				// Dialog comes back as true.
				if (win.ShowDialog() == true)
				{
					// Change the hotkey.
					SelectedItem.Name = vm.Name;
					SelectedItem.preset = GetPresetByName(vm.Preset);
					SelectedItem.Key = (System.Windows.Forms.Keys)Enum.GetValues(typeof(System.Windows.Forms.Keys)).GetValue(vm.Hotkey);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something happened while editing your hotkey!", "Paisley Park", MessageBoxButton.OK, MessageBoxImage.Error);
				logger.Error(ex, "Exception while editing selected item.");
				logger.Error(ex.StackTrace);
			}
		}

		/// <summary>
		/// When you click OK button.
		/// </summary>
		/// <param name="window"></param>
		private void OnOK(Window window)
		{
			// Set that we're saving changes.
			DialogResult = true;
			// Close the window.
			window.Close();
		}

		/// <summary>
		/// Clicking import button.
		/// </summary>
		private void OnImport()
		{
			// Create new import window.
			var import = new Import
			{
				// Owner set to MainWindow.
				Owner = Application.Current.MainWindow
			};

			// Get the view model.
			var vm = import.DataContext as ImportHotkeyViewModel;

			// Add the imported hotkey if it came back okay.
			if (import.ShowDialog() == true && vm.ImportedHotkey != null)
				Hotkeys.Add(vm.ImportedHotkey);
		}

		/// <summary>
		/// Clicking export button.
		/// </summary>
		private void OnExport()
		{
			if (SelectedItem == null)
				return;

			// Serialized string initialize.
			string cereal = "";
			try
			{
				// Serialize the selected item.
				cereal = JsonConvert.SerializeObject(SelectedItem);
				// Set in the clipboard (need to use SetDataObject because SetText crashes).
				Clipboard.SetDataObject(cereal);
				MessageBox.Show(
					string.Format("Copied hotkey \"{0}\" to your clipboard!", SelectedItem.Name),
					"Paisley Park",
					MessageBoxButton.OK,
					MessageBoxImage.Information
				);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Tried to copy serialized object to clipboard.\n---BEGIN---\n{0}\n---END---\n", cereal);
				MessageBox.Show(
					"An error occured while trying to copy this hotkey to your clipboard.",
					"Paisley Park",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			}
		}
	}
}

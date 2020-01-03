using Newtonsoft.Json;
using PaisleyPark.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows;
using System.Windows.Input;

namespace PaisleyPark.ViewModels
{
	public class ImportHotkeyViewModel : BindableBase
	{
		public string ImportText { get; set; }
		public bool? DialogResult { get; set; } = false;
		public ICommand ImportCommand { get; private set; }

		private readonly static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public Models.Hotkey ImportedHotkey;

		public ImportHotkeyViewModel()
		{
			ImportCommand = new DelegateCommand(OnImport);
		}

		private void OnImport()
		{
			logger.Info("Importing JSON");

			try
			{
				// Deserialize JSON into Preset object.
				ImportedHotkey = JsonConvert.DeserializeObject<Models.Hotkey>(ImportText);

				// Checking validity purely by the name being specified. Could use a more robust check but this is good enough.
				if (ImportedHotkey.Name == null || ImportedHotkey.Name.Trim() == string.Empty)
				{
					MessageBox.Show("This does not resemble a valid preset. Could not import successfully.", "Paisley Park", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				MessageBox.Show("Imported Preset " + ImportedHotkey.Name + "!", "Paisley Park", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				DialogResult = true;
			}
			// Likely not a JSON string.
			catch (Exception ex)
			{
				logger.Error(ex, "Error trying to import JSON\n{0}", ImportText);
				MessageBox.Show("Invalid input, this is not valid JSON. Could not import preset.", "Paisley Park", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}

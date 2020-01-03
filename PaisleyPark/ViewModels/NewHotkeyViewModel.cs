using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PaisleyPark.ViewModels
{
	public class NewHotkeyViewModel : BindableBase
	{
		public string Name { get; set; }
		public string Preset { get; set; }
		public int Hotkey { get; set; }
		public bool UseCurrentWaymarks { get; set; } = true;
		public bool? DialogResult { get; private set; }

		public ICommand CreateCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }

		public NewHotkeyViewModel()
		{
			CreateCommand = new DelegateCommand<Window>(OnCreate);
			CancelCommand = new DelegateCommand<Window>(OnCancel);
		}

		/// <summary>
		/// When clicking the create button.
		/// </summary>
		/// <param name="window">Window this was called from.</param>
		private void OnCreate(Window window)
		{
			// Set DidCreate to true.
			DialogResult = true;
			// Close the window.
			if (window != null)
			{
				window.Close();
			}
		}

		/// <summary>
		/// When clicking cancel button.
		/// </summary>
		/// <param name="window">Window this was called from.</param>
		private void OnCancel(Window window)
		{
			// Set DidCreate to false.
			DialogResult = false;
			// Close the window.
			if (window != null)
			{
				window.Close();
			}
		}
	}
}

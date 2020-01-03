using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;

namespace PaisleyPark.Models
{
	/// <summary>
	/// Preset Model for use in the application.
	/// </summary>
	public class Hotkey : INotifyPropertyChanged
	{
		/// <summary>
		/// Name of this hotkey.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Key of this hotkey.
		/// </summary>
		public Keys Key { get; set; }

		/// <summary>
		/// Preset connected to this hotkey
		/// </summary>
		public Preset preset { get; set; }

		/// <summary>
		/// Property Changed event handler for this model.
		/// </summary>
#pragma warning disable 67
		public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67
	}
}


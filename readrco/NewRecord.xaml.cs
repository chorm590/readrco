using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using readrco.src.tool;

namespace readrco
{
	/// <summary>
	/// Interaction logic for NewRecord.xaml
	/// </summary>
	public partial class NewRecord : Window
	{
		private const string TAG = "NewRecord";


		public NewRecord()
		{
			InitializeComponent();

			GBStar.Visibility = Visibility.Collapsed;
			
		}

		private void Read_Status_Watcher(object sender, RoutedEventArgs e)
		{
			Logger.v(TAG, "Read_Status_Watcher()");
		}

		private void Add_Author(object sender, RoutedEventArgs e)
		{
			Logger.v(TAG, "Add_Author()");
			TextBox tbox = new TextBox();
			tbox.Width = 200;

			SPAuthors.Children.Add(tbox);
		}
	}
}

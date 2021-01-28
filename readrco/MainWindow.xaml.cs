using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using readrco.src.tool;

namespace readrco
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string TAG = "readrco";

		private NewRecord newRecordWindow;

		public MainWindow()
		{
			InitializeComponent();
			Logger.v(TAG, "hello world");

		}

		private void Btn_New_Click(object sender, RoutedEventArgs e)
		{
			if(newRecordWindow != null)
			{
				newRecordWindow.Close();
			}

			newRecordWindow = new NewRecord();
			newRecordWindow.Show();
		}

		private void Btn_Edit_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}

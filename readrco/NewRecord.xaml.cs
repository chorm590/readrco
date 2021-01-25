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

		private void Add_Translator(object sender, RoutedEventArgs e)
		{
			Logger.v(TAG, "Add_Translator()");
			SPTranslators.Children.Add(GenRemovableTextBox());
		}

		private void Remove_UIElement_Click(object sender, MouseButtonEventArgs e)
		{
			Logger.v(TAG, "Remove_UIElement_Click()," + e.ChangedButton + ",sender:" + sender);

			if(sender is Image)
			{
				Image img = sender as Image;
				
			}
		}

		private StackPanel GenRemovableTextBox()
		{
			StackPanel panel = new StackPanel();
			panel.Orientation = Orientation.Horizontal;
			panel.Margin = new Thickness
			{
				Top = 5
			};

			TextBox tbox = new TextBox
			{
				Style = (Style)Resources["AddableTextBoxWidth"]
			};

			BitmapImage bimg = new BitmapImage();
			bimg.BeginInit();
			bimg.UriSource = new Uri(@"res/minus.png", UriKind.Relative);
			bimg.EndInit();
			Image img = new Image
			{
				Width = 18,
				Margin = new Thickness
				{
					Left = 5
				},
				Source = bimg
			};
			img.MouseDown += Remove_UIElement_Click;
			
			panel.Children.Add(tbox);
			panel.Children.Add(img);

			return panel;
		}
	}
}

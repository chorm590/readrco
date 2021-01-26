using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
			SPAuthors.Children.Add(GenRemovableTextBox(SPAuthors));
		}

		private void Add_Translator(object sender, RoutedEventArgs e)
		{
			SPTranslators.Children.Add(GenRemovableTextBox(SPTranslators));
		}

		private void Remove_UIElement_Click(object sender, MouseButtonEventArgs e)
		{
			if(sender is Image)
			{
				Image img = sender as Image;
				StackPanel parent = (StackPanel)VisualTreeHelper.GetParent(img);
				StackPanel pparent = (StackPanel)img.Tag;
				pparent.Children.Remove(parent);
			}
		}

		private void HighFrqCharacter_Click(object sender, MouseButtonEventArgs e)
		{
			if(sender is TextBlock)
			{
				TextBlock tb = sender as TextBlock;
				TextBox tbox = GetFocusedTextBox();
				if(tbox != null)
				{
					Logger.v(TAG, "found...");
					tbox.AppendText(tb.Text.Trim());
				}
				else
				{
					Logger.v(TAG, "not found...");
				}
			}
		}

		private StackPanel GenRemovableTextBox(StackPanel parent)
		{
			StackPanel panel = new StackPanel();
			panel.Orientation = Orientation.Horizontal;
			panel.Margin = new Thickness
			{
				Top = 2
			};

			TextBox tbox = new TextBox
			{
				Style = (Style)Resources["NarrowTextBoxWidth"]
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
			img.Tag = parent;
			
			panel.Children.Add(tbox);
			panel.Children.Add(img);

			return panel;
		}

		private TextBox GetFocusedTextBox()
		{
			if(TBMainTitle.IsFocused)
				return TBMainTitle;
			else if(TBSubTitle.IsFocused)
				return TBSubTitle;
			else if(TBPublish.IsFocused)
				return TBPublish;
			else if(TBPubSn.IsFocused)
				return TBPubSn;
			else if(TBBeginDate.IsFocused)
				return TBBeginDate;
			else if(TBEndDate.IsFocused)
				return TBEndDate;
			else
			{
				UIElementCollection children = SPAuthors.Children;
				TextBox tbox;
				foreach(UIElement sp in children)
				{
					tbox = FindFocusedTextBoxInAuthorsAndTranslaters(sp);
					if(tbox != null)
						return tbox;
				}

				children = SPTranslators.Children;
				foreach(UIElement sp in children)
				{
					tbox = FindFocusedTextBoxInAuthorsAndTranslaters(sp);
					if(tbox != null)
						return tbox;
				}
			}

			return null;
		}

		private TextBox FindFocusedTextBoxInAuthorsAndTranslaters(UIElement sp)
		{
			StackPanel spanel;
			if(sp is StackPanel)
			{
				spanel = sp as StackPanel;
				if(spanel.Children.Count is 2)
				{
					if(spanel.Children[0].IsFocused)
					{
						if(spanel.Children[0] is TextBox)
						{
							return (TextBox)spanel.Children[0];
						}
					}
				}
			}

			return null;
		}
	}
}

using System;
using System.DirectoryServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

using readrco.src.model;
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
		}

		private void Read_Status_Watcher(object sender, RoutedEventArgs e)
		{
			if(sender == RBReading)
			{
				GBStar.Visibility = Visibility.Collapsed;
			}
			else if(sender == RBRead)
			{
				GBStar.Visibility = Visibility.Visible;
			}
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
					tbox.AppendText(tb.Text.Trim());
					tbox.SelectionStart = tbox.Text.Length;
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

		private void Today_Click(object sender, RoutedEventArgs e)
		{
			if(sender == TBBD)
			{
				TBBeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
			}
			else if(sender == TBED)
			{
				TBEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
			}
		}

		private void Save_Btn_Click(object sender, RoutedEventArgs e)
		{
			Logger.v(TAG, "Save_Btn_Click()");
			if(TBMainTitle.Text.Length == 0)
			{
				MessageBox.Show("请输入图书主标题");
				return;
			}

			(string[] authors, byte author_count) = GetAuthors();
			Logger.v(TAG, "author_count:" + author_count);
			if(author_count == 0)
			{
				MessageBox.Show("请输入作者名称");
				return;
			}

			if(TBBeginDate.Text.Length == 0)
			{
				MessageBox.Show("请输入起阅日期");
				return;
			}

			if(RBRead.IsChecked is null || RBReading.IsChecked is null)
			{
				MessageBox.Show("请选择阅读状态");
				return;
			}
			bool reading = RBReading.IsChecked.Value;

			if(reading && TBEndDate.Text.Length > 0)
			{
				MessageBox.Show("终读日期与阅读状态冲突");
				return;
			}
			else if(!reading && TBEndDate.Text.Length == 0)
			{
				MessageBox.Show("请输入终读日期");
				return;
			}

			int sidx = CBStart.SelectedIndex;
			if(!reading)
			{
				if(sidx == -1)
				{
					MessageBox.Show("请选择评分");
					return;
				}
			}

			(string[] translators, byte translator_count) = GetTranslators();
			Logger.v(TAG, "translator_count:" + translator_count);
			Record rco = new Record();
			rco.ID = 1;
			
			if(reading)
				rco.Status = Record.STATUS_READING;
			else
				rco.Status = Record.STATUS_READ;

			rco.Comment = TBComment.Text;
			rco.BeginDate = TBBeginDate.Text;
			rco.EndDate = TBEndDate.Text;

			Book book = new Book();
			book.MainTitle = TBMainTitle.Text;
			if(TBSubTitle.Text.Length > 0)
			{
				book.SubTitle = TBSubTitle.Text;
			}

			for(byte i = 0; i < author_count; i++)
			{
				book.AddAuthor(authors[i]);
			}

			for(byte i = 0; i < translator_count; i++)
			{
				book.AddTranslator(translators[i]);
			}

			if(TBPublish.Text.Length > 0)
			{
				book.Press = TBPublish.Text;
			}

			if(TBPubSn.Text.Length > 0)
			{
				book.PressSn = TBPubSn.Text;
			}

			if(TBWords.Text.Length > 0)
			{
				book.WordCount = int.Parse(TBWords.Text);
			}
			
			XmlDocument xml = new XmlDocument();
			xml.Load("records.xml");
			XmlElement root = xml.DocumentElement;
			XmlElement record = xml.CreateElement("record");
			XmlElement id = xml.CreateElement("id");
			XmlText tid = xml.CreateTextNode(rco.ID.ToString());
			id.AppendChild(tid);
			XmlElement bookk = xml.CreateElement("book");
			XmlElement mainTitle = xml.CreateElement("main_title");
			XmlText tmt = xml.CreateTextNode(book.MainTitle);
			mainTitle.AppendChild(tmt);
			bookk.AppendChild(mainTitle);
			record.AppendChild(id);
			record.AppendChild(bookk);
			root.AppendChild(record);
			xml.Save("records.xml");
		}

		private (string[], byte) GetPersons(StackPanel panel)
		{
			UIElement uitmp;
			UIElementCollection uis = panel.Children;
			string[] authors = new string[10];
			byte author_count = 0;
			foreach(UIElement ui in uis)
			{
				if(ui is StackPanel)
				{
					uitmp = ((StackPanel)ui).Children[0];
					if(uitmp is TextBox)
					{
						authors[author_count] = ((TextBox)uitmp).Text;
						if(authors[author_count].Length > 0)
							author_count++;
					}
				}
			}

			return (authors, author_count);
		}

		private (string[], byte) GetAuthors()
		{
			return GetPersons(SPAuthors);
		}

		private (string[], byte) GetTranslators()
		{
			return GetPersons(SPTranslators);
		}
	}
}

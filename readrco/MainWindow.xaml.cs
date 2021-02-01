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

using readrco.src.model;
using readrco.src.tool;
using readrco.src.xml;

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

			if(XMLManager.Init())
			{
				if(XMLManager.LoadRecord())
				{
					Logger.v(TAG, "Load record finished, count:" + XMLManager.GetRecords().Count);
					foreach(Record rco in XMLManager.GetRecords())
					{
						Logger.v(TAG, rco.ToString());
						LVList.Items.Add(rco);
					}
				}
				else
				{
					MessageBox.Show("读取记录时发生错误");
					Application.Current.Shutdown();
				}
			}
			else
			{
				MessageBox.Show("无法初始化读书记录数据文件");
				Application.Current.Shutdown();
			}

			LVList.MouseDoubleClick += LVList_MouseDoubleClick;
		}

		private void LVList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if(e.ChangedButton == MouseButton.Left)
			{
				Btn_Edit_Click(sender, e);
			}
		}

		private void Btn_New_Click(object sender, RoutedEventArgs e)
		{
			if(newRecordWindow != null)
			{
				newRecordWindow.Close();
			}

			newRecordWindow = new NewRecord(null, GetCurMaxID());
			newRecordWindow.Show();
		}

		private void Btn_Edit_Click(object sender, RoutedEventArgs e)
		{
			if(LVList.SelectedItem is Record rco)
			{
				Logger.v(TAG, "Editing the record of " + rco.Book.MainTitle);
				if(newRecordWindow != null)
				{
					newRecordWindow.Close();
				}

				newRecordWindow = new NewRecord(rco, GetCurMaxID());
				newRecordWindow.Show();
			}
		}

		private int GetCurMaxID()
		{
			int max = 0;
			List<Record> records = XMLManager.GetRecords();
			for(int i = 0; i < records.Count; i++)
			{
				if(max < records[i].ID)
					max = records[i].ID;
			}

			return max;
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

using readrco.src.tool;

namespace readrco.src.xml
{
	internal static class XMLManager
	{
		private const string TAG = "XMLManager";

		private const string RECORD_FILE_NAME = "records.xml";
		private const string ROOT_NODE_NAME = "readrco";

		private static XmlDocument xml;

		/// <summary>
		/// 保证记录存储文件的存在且具有正确的根节点。
		/// 2021-01-29 22:57
		/// </summary>
		internal static bool Init()
		{
			bool isFileEmpty;
			//1. make sure the file exist
			if(File.Exists(RECORD_FILE_NAME))
			{
				isFileEmpty = (new FileInfo(RECORD_FILE_NAME).Length == 0);
			}
			else
			{
				try
				{
					File.Create(RECORD_FILE_NAME).Close();
					isFileEmpty = true;
				}
				catch(Exception e)
				{
					Logger.v(TAG, e.StackTrace);
					return false;
				}
			}

			//2. make sure root node valid
			xml = new XmlDocument();
			if(isFileEmpty)
			{
				if(!CreateRootNode())
				{
					Logger.v(TAG, "Can't create the root node, init failed");
					return false;
				}
			}
			else
			{
				try
				{
					xml.Load(RECORD_FILE_NAME);
					Logger.v(TAG, "xml load success");
				}
				catch(Exception)
				{
					//根节点数据错误。
					try
					{
						File.Move(RECORD_FILE_NAME, RECORD_FILE_NAME + "_" + DateTime.Now.ToString("yyyyMMddhhmmss"));
					}
					catch(Exception e1)
					{
						Logger.v(TAG, "Can't rename the invalid record file.\n" + e1.StackTrace);
						return false;
					}

					try
					{
						File.Create(RECORD_FILE_NAME).Close();
						if(!CreateRootNode())
						{
							Logger.v(TAG, "Can't create the root node, init failed2");
							return false;
						}

						// reload the xml file, 2021-01-31 10:22
						xml.Load(RECORD_FILE_NAME);
						Logger.v(TAG, "reload xml file success");
					}
					catch(Exception e1) {
						Logger.v(TAG, "Unpredictable error occur\n" + e1.StackTrace);
						return false;
					}
				}
			}

			return true;
		}

		private static bool CreateRootNode()
		{
			XmlElement root = xml.CreateElement(ROOT_NODE_NAME);
			try
			{
				xml.AppendChild(root);
				xml.Save(RECORD_FILE_NAME);
			}
			catch(Exception e)
			{
				return false;
			}

			return true;
		}
	}
}

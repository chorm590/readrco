using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

using readrco.src.model;
using readrco.src.tool;

namespace readrco.src.xml
{
	internal static class XMLManager
	{
		private const string TAG = "XMLManager";

		private const string RECORD_FILE_NAME = "records.xml";
		private const string ROOT_NODE_NAME = "readrco";

		private static XmlDocument xml;
		private static List<Record> records;

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
					catch(Exception e1)
					{
						Logger.v(TAG, "Unpredictable error occur\n" + e1.StackTrace);
						return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// load record from xml file to memory
		/// 2021-01-31 17:27
		/// </summary>
		internal static bool LoadRecord()
		{
			if(xml is null)
			{
				Logger.v(TAG, "The 'xml' is null");
				return false;
			}

			XmlElement root = xml.DocumentElement;
			if(root is null)
			{
				Logger.v(TAG, "No root node found");
				return false;
			}

			records = new List<Record>(50);
			XmlNode node = root.FirstChild;
			XmlNode node2;
			XmlNodeList nodes;
			XmlNodeList nodes2;
			Record rco;
			Book book;
			while(node != null)
			{
				if(!node.Name.Equals(Record.NODE_RECORD_NAME))
					continue;

				nodes = node.ChildNodes;
				Logger.v(TAG, "Record child count:" + nodes.Count);
				if(nodes.Count != 3) //2021-01-31 17:58
				{
					continue;
				}

				rco = new Record();
				for(byte i = 0; i < 3; i++)
				{
					node2 = nodes[i];
					if(node2 is null)
					{
						rco = null;
						break;
					}

					Logger.v(TAG, "name:" + node2.Name + ", inner text:" + node2.InnerText);
					if(node2.Name.Equals(Record.NODE_ID_NAME))
					{
						try
						{
							int id = int.Parse(node2.InnerText);
							Logger.v(TAG, "ID:" + id);
							rco.ID = id;
						}
						catch(Exception)
						{
							rco = null;
							break;
						}
					}
					else if(node2.Name.Equals(Record.NODE_BOOK_NAME))
					{
						nodes2 = node2.ChildNodes;
						Logger.v(TAG, "'book' child count:" + nodes2.Count);

						book = new Book();
						for(byte j = 0; j < nodes2.Count; j++)
						{
							switch(nodes2[j].Name)
							{
								case Record.NODE_MTITLE_NAME:
									book.MainTitle = nodes2[j].InnerText;
									break;
								case Record.NODE_STITLE_NAME:
									book.SubTitle = nodes2[j].InnerText;
									break;
								case Record.NODE_AUTHORS_NAME:
									XmlNodeList nodes3 = nodes2[j].ChildNodes;
									for(byte k = 0; k < nodes3.Count; k++)
									{
										book.AddAuthor(nodes3[k].InnerText);
									}
									break;
								case Record.NODE_TRANSLATORS_NAME:
									XmlNodeList nodes4 = nodes2[j].ChildNodes;
									for(byte k = 0; k < nodes4.Count; k++)
									{
										book.AddTranslator(nodes4[k].InnerText);
									}
									break;
								case Record.NODE_PRESS_NAME:
									book.Press = nodes2[j].InnerText;
									break;
								case Record.NODE_PRESSSN_NAME:
									book.PressSn = nodes2[j].InnerText;
									break;
								case Record.NODE_WORDCOUNT_NAME:
									try
									{
										book.WordCount = float.Parse(nodes2[j].InnerText);
									}
									catch(Exception)
									{
										Logger.v(TAG, "Invalid word-count found");
										book = null;
									}
									break;
							} //switch -- end

							if(book is null)
								break;
						} //for -- end

						if(book is null)
						{
							rco = null;
							break;
						}
						else
						{
							rco.Book = book;
						}
					}
					else if(node2.Name.Equals(Record.NODE_RINFO_NAME))
					{
						nodes2 = node2.ChildNodes;
						Logger.v(TAG, "'read-info' child count:" + nodes2.Count);
						for(byte j = 0; j < nodes2.Count; j++)
						{
							switch(nodes2[j].Name)
							{
								case Record.NODE_STATUS_NAME:
									if(nodes2[j].InnerText.Equals("0"))
									{
										rco.Status = Record.STATUS_READING;
									}
									else
									{
										rco.Status = Record.STATUS_READ;
									}
									break;
								case Record.NODE_BEGINDATE_NAME:
									rco.BeginDate = nodes2[j].InnerText;
									break;
								case Record.NODE_ENDDATE_NAME:
									rco.EndDate = nodes2[j].InnerText;
									break;
								case Record.NODE_STAR_NAME:
									try
									{
										rco.Star = byte.Parse(nodes2[j].InnerText);
									}
									catch(Exception)
									{
										rco = null;
									}
									break;
								case Record.NODE_COMMENT_NAME:
									rco.Comment = nodes2[j].InnerText;
									break;
							} //switch -- end.

							if(rco is null)
							{
								break;
							}
						} //for -- end
					}
					else
					{
						Logger.v(TAG, "Invalid node in 'record' found");
						rco = null;
						break;
					}
				} //for -- end

				if(rco != null)
				{
					records.Add(rco);
				}

				node = node.NextSibling;
			} //while -- end

			return true;
		}

		internal static List<Record> GetRecords()
		{
			return records;
		}

		internal static void InsertRecord(XmlNode node)
		{
			if(xml is null)
				return;

			XmlElement root = xml.DocumentElement;
			if(root is null)
				return;

			root.InsertBefore(node, root.FirstChild);
			xml.Save(RECORD_FILE_NAME);
			Logger.v(TAG, "storaged");
		}

		internal static XmlElement GetNewElement(string name)
		{
			if(xml is null)
				return null;

			return xml.CreateElement(name);
		}

		internal static XmlText GetNewTextNode(string value)
		{
			if(xml is null)
				return null;

			return xml.CreateTextNode(value);
		}

		private static bool CreateRootNode()
		{
			XmlElement root = xml.CreateElement(ROOT_NODE_NAME);
			try
			{
				xml.AppendChild(root);
				xml.Save(RECORD_FILE_NAME);
			}
			catch(Exception)
			{
				return false;
			}

			return true;
		}
	}
}

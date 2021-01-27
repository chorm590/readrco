﻿using System;
using System.Collections.Generic;
using System.Text;

namespace readrco.src.model
{
	internal class Book
	{
		private byte author_count;
		private byte translator_count;

		private string[] authors;
		private string[] translators;

		internal Book()
		{
			authors = new string[1];
			translators = new string[1];
		}

		internal string MainTitle
		{
			get;
			set;
		}

		internal string SubTitle
		{
			get;
			set;
		}

		internal string Press
		{
			get;
			set;
		}

		internal string PressSn
		{
			get;
			set;
		}

		/// <summary>
		/// kilo word count
		/// </summary>
		internal float WordCount
		{
			get;
			set;
		}

		internal string[] GetAuthors()
		{
			return authors;
		}

		/// <summary>
		/// 允许添加重复名称。
		/// </summary>
		internal void AddAuthor(string author)
		{
			string[] tmp = AddString(author, authors, ref author_count);
			if(tmp != null)
			{
				authors = tmp;
			}
		}

		internal void RemoveAuthor(string author)
		{
			string[] tmp = RemoveString(author, authors, ref author_count);
			if(tmp != null)
			{
				authors = tmp;
			}
		}

		internal string[] GetTranslators()
		{
			return translators;
		}

		internal void AddTranslator(string translator)
		{
			string[] tmp = AddString(translator, translators, ref translator_count);
			if(tmp != null)
				translators = tmp;
		}

		internal void RemoveTranslator(string translator)
		{
			string[] tmp = RemoveString(translator, translators, ref translator_count);
			if(tmp != null)
			{
				translators = tmp;
			}
		}

		private string[] AddString(string str, string[] which, ref byte which_count)
		{
			if(str == null || str.Length == 0 || which == null)
				return null;

			if(which_count < which.Length)
			{
				which[author_count] = str;
				which_count++;
				return which;
			}
			else
			{
				//expand array
				string[] tmp = new string[which_count + 1];
				for(byte i = 0; i < which_count; i++)
				{
					tmp[i] = which[i];
				}

				which_count++;
				tmp[which_count] = str;

				return tmp;
			}
		}

		private string[] RemoveString(string str, string[] which, ref byte which_count)
		{
			if(str == null || str.Length == 0 || which == null || which_count == 0)
				return null;

			string[] tmp = new string[which_count];
			byte j = 0;
			for(byte i = 0; i < which_count; i++)
			{
				if(!which[i].Equals(str))
				{
					tmp[j++] = which[i];
					//一路删下去
				}
			}

			which_count = j;
			return tmp;
		}
	}
}

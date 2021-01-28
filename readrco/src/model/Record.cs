using System;
using System.Collections.Generic;
using System.Text;

namespace readrco.src.model
{
	internal class Record
	{
		internal const byte STATUS_READING = 0;
		internal const byte STATUS_READ = 1;

		internal int ID
		{
			get;
			set;
		}

		internal Book book
		{
			get;
			set;
		}

	    internal byte Status
		{
			get;
			set;
		}

		internal string BeginDate
		{
			get;
			set;
		}

		internal string EndDate
		{
			get;
			set;
		}

		internal byte Star
		{
			get;
			set;
		}

		internal string Comment
		{
			get;
			set;
		}

		public override string ToString()
		{
			if(book is null)
				return "";

			return "ID:" + ID + "\nMainTitle:" + book.MainTitle + "\nSubTitle:" + book.SubTitle + "\nStar:" + Star + "\nComment:" + Comment;
		}
	}
}

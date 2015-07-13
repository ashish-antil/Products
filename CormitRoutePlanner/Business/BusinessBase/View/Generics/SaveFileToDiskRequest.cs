using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace FernBusinessBase
{
	/// <summary>
	/// Enacpsulates a request to save the given File to  Disk.
	/// </summary>
	/// <typeparam name="T">The file name and data that need to be saved.</typeparam>
	[MessageContract]
	public class SaveFileToDiskRequest /*: BusinessEntity*/
	{
		private string _FileName;
		private byte[] _FileData;

		private int _FileLength;
		private int _BytesRead;
		private int _Offset;
		bool _Append;
		   

		[MessageBodyMember]
		public string FileName
		{
			get { return _FileName; }
			set { _FileName = value; }
		}

		[MessageBodyMember]
		public byte[] FileData
		{
			get { return _FileData; }
			set { _FileData = value; }
		}

		[MessageBodyMember]
		public int FileLength
		{
			get { return _FileLength; }
			set { _FileLength = value; }
		}

		[MessageBodyMember]
		public int BytesRead
		{
			get { return _BytesRead; }
			set { _BytesRead = value; }
		}

		[MessageBodyMember]
		public int Offset
		{
			get { return _Offset; }
			set { _Offset = value; }
		}


		[MessageBodyMember]
		public bool Append
		{
			get { return _Append; }
			set { _Append = value; }
		}
	}
}

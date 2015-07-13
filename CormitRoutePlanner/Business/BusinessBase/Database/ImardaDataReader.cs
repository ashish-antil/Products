
using System;
using System.Data;
using System.Diagnostics;
using Imarda.Lib;

// ReSharper disable once CheckNamespace
namespace FernBusinessBase
{
	/// <summary>
	/// Wrapper for any IDataReader, making lookup column by name more efficient.
	/// </summary>
	public sealed class ImardaDataReader : IDataReader
	{
        // do not do this in production
        //private static HashSet<string> _SPs;


		private readonly IDataReader _Reader;
		private int _Index;
		private readonly string _StoredProcName;
		private readonly Stopwatch _StopWatch;
		private readonly static long _Threshold = ConfigUtils.GetInt("DbCallDurationMin", 5);

		public ImardaDataReader(IDataReader reader)
		{
			_Reader = reader;
		}

		public ImardaDataReader(IDataReader reader, string storedProcedureName, Stopwatch stopwatch)
			: this(reader)
		{
			if (ImardaDatabase.Log != null)
			{
				if (stopwatch != null)
				{
					_StoredProcName = storedProcedureName;
					_StopWatch = stopwatch;
				}
				else
				{
					ImardaDatabase.Log.Info(storedProcedureName);
				}
				//if (_SPs == null) _SPs = new HashSet<string>();
			}
		}

		public void Dispose()
		{
			if (_StopWatch != null)
			{
				long ms = _StopWatch.ElapsedMilliseconds;
				if (ms >= _Threshold) ImardaDatabase.Log.InfoFormat("{0}ms {1}", ms, _StoredProcName);
			}
			_Reader.Dispose();
		}

		public string GetName(int i)
		{
			return _Reader.GetName(i);
		}

		public string GetDataTypeName(int i)
		{
			return _Reader.GetDataTypeName(i);
		}

		public Type GetFieldType(int i)
		{
			return _Reader.GetFieldType(i);
		}

		public object GetValue(int i)
		{
			return _Reader.GetValue(i);
		}

		public int GetValues(object[] values)
		{
			return _Reader.GetValues(values);
		}

		public int GetOrdinal(string name)
		{
			int i = IndexOf(name);
			if (i != -1) _Index = i;
			return i;
		}

		/// <summary>
		/// The normal order of addressing columns after a Read() is in the order that 
		/// they are provided in the result set. So if we remember the last index in _Index 
		/// then most of the time we just have to test just the next field instead of starting the loop at 0
		/// </summary>
		/// <param name="field">name of field (column)</param>
		/// <returns>index of field, or -1 if not found</returns>
		public int IndexOf(string field)
		{
			int i = _Index;
			while (i < _Reader.FieldCount)
			{
				if (string.Compare(_Reader.GetName(i), field, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
				i++;
			}
			i = 0;
			while (i < _Index)
			{
				if (string.Compare(_Reader.GetName(i), field, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		public bool GetBoolean(int i)
		{
			return _Reader.GetBoolean(i);
		}

		public byte GetByte(int i)
		{
			return _Reader.GetByte(i);
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return _Reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
		}

		public char GetChar(int i)
		{
			return _Reader.GetChar(i);
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return _Reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
		}

		public Guid GetGuid(int i)
		{
			return _Reader.GetGuid(i);
		}

		public short GetInt16(int i)
		{
			return _Reader.GetInt16(i);
		}

		public int GetInt32(int i)
		{
			return _Reader.GetInt32(i);
		}

		public long GetInt64(int i)
		{
			return _Reader.GetInt64(i);
		}

		public float GetFloat(int i)
		{
			return _Reader.GetFloat(i);
		}

		public double GetDouble(int i)
		{
			return _Reader.GetDouble(i);
		}

		public string GetString(int i)
		{
			return _Reader.GetString(i);
		}

		public decimal GetDecimal(int i)
		{
			return _Reader.GetDecimal(i);
		}

		public DateTime GetDateTime(int i)
		{
			return _Reader.GetDateTime(i);
		}

		public IDataReader GetData(int i)
		{
			return _Reader.GetData(i);
		}

		public bool IsDBNull(int i)
		{
			return _Reader.IsDBNull(i);
		}

		public int FieldCount
		{
			get { return _Reader.FieldCount; }
		}

		public object this[int i]
		{
			get { return _Reader[i]; }
		}

		public object this[string name]
		{
			get
			{
				int i = IndexOf(name);
				if (i == -1) return null;
				_Index = i + 1;
				return _Reader[i];
			}
		}

		public void Close()
		{
			_Reader.Close();
		}

		public DataTable GetSchemaTable()
		{
			return _Reader.GetSchemaTable();
		}

		public bool NextResult()
		{
			return _Reader.NextResult();
		}

		public bool Read()
		{
			_Index = 0;
			bool b = _Reader.Read();

// Do not do this in production, this adds unnecessary thread locks and memory leak
//			if (_SPs != null)
//			{
//				lock (_SPs)
//				{
//					if (!_SPs.Contains(_StoredProcName))
//					{
//						var columns = new HashSet<string>();
//						for (int i = 0; i < _Reader.FieldCount; i++)
//						{
//							string name = _Reader.GetName(i);
//							if (columns.Contains(name))
//							{
//								var msg = string.Format("Duplicate column name '{1}' returned by '{0}'", _StoredProcName, name);
//								ImardaDatabase.Log.ErrorFormat(msg);
//							}
//							columns.Add(name);
//						}
//						_SPs.Add(_StoredProcName);
//					}
//				}
//			}
			return b;
		}

		public int Depth
		{
			get { return _Reader.Depth; }
		}

		public bool IsClosed
		{
			get { return _Reader.IsClosed; }
		}

		public int RecordsAffected
		{
			get { return _Reader.RecordsAffected; }
		}

	}

}

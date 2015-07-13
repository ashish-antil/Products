using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
    [Obsolete("Use System.Collections.BitArray instead")]
    [DataContract]
	[Serializable]
	public class BitMatrix64
	{
		public const int Dim0 = 64;
		[DataMember] private long[] _Cells;

		public BitMatrix64()
		{
		}

		/// <summary>
		/// Create a 64 x n bitmatrix
		/// </summary>
		/// <param name="n">n is the number of elements in j-dimension: cell[i,j]</param>
		public BitMatrix64(int n)
		{
			Initialize(n);
		}

		public BitMatrix64(long[] data)
		{
			_Cells = data;
		}

		public int Dim1
		{
			get { return _Cells.Length; }
		}


		public bool this[int i, int j]
		{
			get
			{
				long rows = _Cells[j];
				long mask = 1L << i;
				return (rows & mask) != 0;
			}
			set
			{
				long mask = 1L << i;
				if (value) _Cells[j] |= mask;
				else _Cells[j] &= ~mask;
			}
		}

		public bool IsEmpty
		{
			get
			{
				if (_Cells == null) return true;
				return _Cells.All(x => x == 0L);
			}
		}

		public void Initialize(int n)
		{
			_Cells = new long[n];
		}

		public void ResetAll()
		{
			_Cells = new long[_Cells.Length];
		}

		public void SetAll()
		{
			_Cells = new long[_Cells.Length];
			for (int i = 0; i < _Cells.Length; i++) _Cells[i] = -1L;
		}

		public void SetRow(int row, bool value)
		{
			for (int i = 0; i < _Cells.Length; i++) this[row, i] = value;
		}

		public void SetColumn(int col, bool value)
		{
			_Cells[col] = -1L;
		}

		/// <summary>
		/// Is any of the rows set to true in the given column?
		/// </summary>
		/// <param name="col"></param>
		/// <returns></returns>
		public bool ColumnAny(int col)
		{
			return _Cells[col] != 0L;
		}

		/// <summary>
		/// Is any of the columns set to true in the given row?
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		public bool RowAny(int row)
		{
			return _Cells.Where((t, col) => this[row, col]).Any();
		}


		//internal void Print()
		//{
		//  for (int j = 0; j < 64; j++)
		//  {
		//    Console.Write("{0,-2} ->   ", j);
		//    for (int i = 0; i < _Cells.Length; i++)
		//    {
		//      Console.Write("{0}", this[j, i] ? "1" : "0");
		//    }
		//    Console.WriteLine();
		//  }
		//}
	}
}
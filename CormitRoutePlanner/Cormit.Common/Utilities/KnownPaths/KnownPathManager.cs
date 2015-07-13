using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Imarda.Lib.MVVM.Extensions;

namespace Imarda.Lib.Utilities.KnownPaths
{
	public class KnownPathManager : KnownPathManagerBase
	{
	}

	public abstract class KnownPathManagerBase : IKnownPathManager
	{
		private List<string> _knownPaths;

		protected KnownPathManagerBase()
		{
			Create = path =>
			{
				var di = Directory.CreateDirectory(path);
				return di.Exists;
			};

			CanFlush = path => true;
		}

		protected List<string> KnownPaths { get { return _knownPaths ?? (_knownPaths = new List<string>()); } }

		public bool PathExists(string path)
		{
			return KnownPaths.Contains(path);
		}

		public virtual bool CreatePath(string path)
		{
			var result = Create(path);
			if (result && !PathExists(path))
			{
				KnownPaths.Add(path);
				Flush();
			}
			return result;
		}

		public bool CreatePathIfNotExists(string path)
		{
			if (PathExists(path))
			{
				return true;
			}
			return CreatePath(path);
		}

		/// <summary>
		/// Removes all by the last 10 known paths in internal dictionary
		/// </summary>
		/// <param name="param"></param>
		public virtual void Flush(int param = 10)
		{
			var count = KnownPaths.Count;
			count -= param;
			if (count<=0) { return; }
			var removables = _knownPaths.Where(path => CanFlush(path)).Take(count).ToList();
			removables.ForEach(dtkey => _knownPaths.Remove(dtkey));
		}

		/// <summary>
		/// Defaults to Directory.Create
		/// </summary>
		public Func<string,bool> Create { get; protected set; }

		/// <summary>
		/// Defaults to true
		/// </summary>
		public Func<string, bool> CanFlush { get; protected set; }
	}

}

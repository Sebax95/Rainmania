using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
//using UnityEngine;

namespace CustomMSLibrary.Core {

	/// <summary>
	/// A list with it's own index with a Next method and a looping behaviour, preventing Out Of Index errors unless the count is 0.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public class LoopingList<T> {
		private int index = 0;
		private List<T> list;
		public List<T> List {
			get {
				return list;
			}
		}

		/// <summary>
		/// Points to LAST USED value.
		/// Setting this index to point a value and using 'Next' will result in returning the value indexed after the set one.
		/// </summary>
		public int Index {
			get {
				return index;
			}
			set {
				value = (value % list.Count);
				index = value;
			}
		}

		/// <summary>
		/// Count of items in the list.
		/// </summary>
		public int Count {
			get {
				return List.Count;
			}
		}

		public LoopingList() {
			list = new List<T>();
		}

		public LoopingList(List<T> l) {
			list = new List<T>(l);
		}

		public T Current => list[Index];

		/// <summary>
		/// Returns next value from the loop. This updates its internal index, then returns value.
		/// </summary>
		public T Next {
			get {
				Index += 1;
				return list[Index];
			}
		}

		public void Add(T value) {
			list.Add(value);
		}

		public void Remove(T value) {
			list.Remove(value);
		}

		/// <summary>
		/// CAREFUL: Removing at a defined index still uses the looping behaviour.
		/// Be sure to know which element you're trying to delete after the looping behaviour!
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index) {
			index = (index % Count);
			list.RemoveAt(index);
			if(Index >= index && Index > 0)
				Index--;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="newList"></param>
		/// <param name="resetIndex"></param>
		public void Replace(List<T> newList, bool resetIndex = true) {
			list = new List<T>(newList);
			if(resetIndex)
				index = 0;
		}

		public static explicit operator LoopingList<T>(List<T> l) {
			return new LoopingList<T>(l);
		}

		public static implicit operator List<T>(LoopingList<T> l) {
			return l.List;
		}
	}

}


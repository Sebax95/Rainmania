//using UnityEngine;

namespace CustomMSLibrary {

	namespace Core {
		/// <summary>
		/// Custom version of Tuples, in case an older version of .NET is used
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		public struct Duple<T1, T2> {
			public T1 V1;
			public T2 V2;

			public Duple(T1 v1, T2 v2) {
				V1 = v1;
				V2 = v2;
			}

		}

	}

}

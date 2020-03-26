using System;
//using UnityEngine;

namespace CustomMSLibrary {

	namespace Core {
		/// <summary>
		/// Structure containing 8 boolean values stored inside a byte.
		/// Intended to optimize the use of boolean values in memory at the cost of processing.
		/// </summary>
		public struct BoolByte {
			private byte data;

			public byte RawData { get => data; }

			/// <summary>
			/// Create new BoolByte package from boolean values.
			/// </summary>
			/// <param name="data">Boolean values.</param>
			public BoolByte(params bool[] data) {
				int length;
				if (data.Length > 8)
				{
					length = 8;
					throw new ArgumentException("BoolByte created was provided with more than 8 values. Values with index 8 and above ignored.");
				} else
					length = data.Length;
				this.data = 0;
				for (int i = 0; i < length; i++)
				{
					this.data = this.data.SetBit(i, data[i]);
				}
			}

			/// <summary>
			/// Create new BoolByte package from a preexisting byte.
			/// </summary>
			/// <param name="data">Byte value.</param>
			public BoolByte(byte data) {
				this.data = data;
			}

			public bool this[int i] {
				get
				{
					if (i < 0 || i > 7) throw new System.IndexOutOfRangeException();
					return data.GetBit(i);
				}
				set
				{
					if (i < 0 || i > 7) throw new System.IndexOutOfRangeException();
					data = data.SetBit(i, value);
				}
			}

			#region Overrides
			public override bool Equals(object obj)
			{
				return (data.Equals(obj) && (obj.GetType() == typeof(BoolByte)));
			}

			public override int GetHashCode()
			{
				return (((data.GetHashCode() + 14388) * 3)-2155)/4;
			}
			#endregion

			#region Operators
			public static bool operator ==(BoolByte l, BoolByte r) => l.data == r.data;
			public static bool operator !=(BoolByte l, BoolByte r) => l.data != r.data;
			#endregion
		}

	}

}

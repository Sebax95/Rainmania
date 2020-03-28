using System;
//using UnityEngine;

namespace CustomMSLibrary {

	namespace Core {
		/// <summary>
		/// Miscelaneous extension methods
		/// </summary>
		public static class CustomExtensions {

			/// <summary>
			/// Get individual bit from a byte.
			/// </summary>
			/// <param name="input">Byte to extract boolean bit from.</param>
			/// <param name="index">Index from 0 to 7 of the desired bit within the byte.</param>
			/// <returns></returns>
			public static bool GetBit(this byte input, int index) {
				if (index < 0 || index > 7) throw new IndexOutOfRangeException();
				return (input & (1 << index)) != 0;
			}

			/// <summary>
			/// Set individual bit to a byte. This does not modifies the referenced byte.
			/// </summary>
			/// <param name="thisByte">Byte to insert bit into.</param>
			/// <param name="index">Index from 0 to 7 of the desired bit within the byte.</param>
			/// <param name="value">Boolean value to set bit as (0 or 1).</param>
			/// <returns></returns>
			public static byte SetBit(this byte thisByte, int index, bool value)
			{
				if (index < 0 || index > 7) throw new IndexOutOfRangeException();
				if (value)
					thisByte = (byte)(thisByte | 1 << index);
				else
					thisByte = (byte)(thisByte & ~(1 << index));
				return thisByte;
			}

			/// <summary>
			/// Same as SetBit, but instead actually modifies the referenced byte.
			/// Set individual bit to a byte.
			/// </summary>
			/// <param name="thisByte">Byte to insert bit into.</param>
			/// <param name="index">Index from 0 to 7 of the desired bit within the byte.</param>
			/// <param name="value">Boolean value to set bit as (0 or 1).</param>
			public static void RefSetBit(this ref byte thisByte, int index, bool value) =>
				thisByte = thisByte.SetBit(index, value);

			/// <summary>
			/// Get individual bit from an int.
			/// </summary>
			/// <param name="input">Int to extract boolean bit from.</param>
			/// <param name="index">Index from 0 to 31 of the desired bit within the int.</param>
			/// <returns></returns>
			public static bool GetBit(this int input, int index) {
				if (index < 0 || index > 31) throw new IndexOutOfRangeException();
				return (input & (1 << index)) != 0;
			}

			/// <summary>
			/// Set individual bit to an in. This does not modifies the referenced int.
			/// </summary>
			/// <param name="input">Int to insert bit into.</param>
			/// <param name="value">Boolean value to set bit as (0 or 1).</param>
			/// <param name="index">Index from 0 to 31 of the desired bit within the byte.</param>
			/// <returns></returns>
			public static int SetBit(this int input, int index, bool value) {
				if (index < 0 || index > 31) throw new IndexOutOfRangeException();
				if (value)
					input = (byte)(input | 1 << index);
				else
					input = (byte)(input & ~(1 << index));
				return input;
			}

			/// <summary>
			/// Get a slice of an array as a new array.
			/// </summary>
			/// <param name="source">Source array from which the slice will be made.</param>
			/// <param name="start">Index from the original array from which to begin the slice.</param>
			/// <param name="count">Count of elements to copy from the array.</param>
			/// <returns></returns>
			public static T[] Slice<T>(this T[] source, int start, int count) {
				var array = new T[count];
				float limit = count + start;
				int c = 0;
				for (int i = start; i < limit; i++)
				{
					array[c] = source[i];
					c++;
				}
				return array;
			}

			/// <summary>
			/// Returns value squared.
			/// </summary>
			/// <returns></returns>
			public static float Squared(this float num) => num * num;

		}

	}

}

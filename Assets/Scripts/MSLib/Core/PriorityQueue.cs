using System.Collections.Generic;
//using UnityEngine;

namespace CustomMSLibrary {

	namespace Core {
		/// <summary>
		/// Priority Queue
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public class PriorityQueue<T> {
			private List<(T item, float weight)> queue;
			private readonly bool greaterFirst;

			public int Count {
				get
				{
					return queue.Count;
				}
			}

			public PriorityQueue(bool priorityIsGreater = true) {
				queue = new List<(T item, float weight)>();
				greaterFirst = priorityIsGreater;
			}

			public void Enqueue(T item, float cost) {
				queue.Add((item, cost));
			}

			public void Enqueue((T item, float weight) item) {
				queue.Add(item);
			}

			public T Dequeue() {
				if (greaterFirst)
					return SearchGreater().item;
				else
					return SearchLesser().item;
			}

			private (T item, float weight) SearchLesser() {
				float testValue = float.PositiveInfinity;
				int testIndex = -1;
				int queueCount = queue.Count;
				for (int i = 0; i < queueCount; i++)
				{
					if (queue[i].weight <= testValue)
					{
						testIndex = i;
						testValue = queue[i].weight;
					}
				}
				if (testIndex == -1)
					return default;
				(T item, float weight) returned = queue[testIndex];
				queue.RemoveAt(testIndex);
				return returned;

			}

			private (T item, float weight) SearchGreater() {
				float testValue = float.NegativeInfinity;
				int testIndex = -1;
				int queueCount = queue.Count;
				for (int i = 0; i < queueCount; i++)
				{
					if (queue[i].weight >= testValue)
					{
						testIndex = i;
						testValue = queue[i].weight;
					}
				}
				if(testIndex == -1)
					return default;
				(T item, float weight) returned = queue[testIndex];
				queue.RemoveAt(testIndex);
				return returned;

			}

			public void Clear() =>
				queue.Clear();

			public bool Contains(T item) {
				int c = queue.Count;
				for (int i = 0; i < c; i++)
				{
					if (queue[i].item.Equals(item))
						return true;
				}
				return false;
			}
		}

	}

}

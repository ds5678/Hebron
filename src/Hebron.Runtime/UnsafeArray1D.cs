using System;
using System.Runtime.InteropServices;

namespace Hebron.Runtime
{
	public unsafe class UnsafeArray1D<T> where T : struct
	{
		internal GCHandle PinHandle { get; }

		public T this[int index]
		{
			get => Data[index];
			set
			{
				Data[index] = value;
			}
		}

		public T[] Data { get; }

		public UnsafeArray1D(int size)
		{
			if (size < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(size));
			}

			Data = new T[size];
			PinHandle = GCHandle.Alloc(Data, GCHandleType.Pinned);
		}

		public UnsafeArray1D(T[] data, int sizeOf)
		{
			if (sizeOf <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(sizeOf));
			}

			Data = data ?? throw new ArgumentNullException(nameof(data));
			PinHandle = GCHandle.Alloc(Data, GCHandleType.Pinned);
		}

		~UnsafeArray1D()
		{
			PinHandle.Free();
		}

		public void* ToPointer()
		{
			return PinHandle.AddrOfPinnedObject().ToPointer();
		}

		public static implicit operator void*(UnsafeArray1D<T> array)
		{
			return array.ToPointer();
		}

		public static void* operator +(UnsafeArray1D<T> array, int delta)
		{
			return array.ToPointer();
		}
	}
}

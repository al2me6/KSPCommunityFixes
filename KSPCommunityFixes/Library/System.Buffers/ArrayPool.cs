using System.Buffers;
using System.Runtime.CompilerServices;
using System.Threading;

namespace KSPCommunityFixes.Library.Buffers
{
    /// <summary>Provides a resource pool that enables reusing instances of type <see cref="T[]"></see>.</summary>
    /// <typeparam name="T">The type of the objects that are in the resource pool.</typeparam>
    public abstract class ArrayPool<T>
    {
        /// <summary>Creates a new instance of the <see cref="T:System.Buffers.ArrayPool`1"></see> class.</summary>
        /// <returns>A new instance of the <see cref="System.Buffers.ArrayPool`1"></see> class.</returns>
        public static ArrayPool<T> Create()
        {
            return new DefaultArrayPool<T>();
        }

        /// <summary>Creates a new instance of the <see cref="T:System.Buffers.ArrayPool`1"></see> class using the specifed configuration.</summary>
        /// <param name="maxArrayLength">The maximum length of an array instance that may be stored in the pool.</param>
        /// <param name="maxArraysPerBucket">The maximum number of array instances that may be stored in each bucket in the pool. The pool groups arrays of similar lengths into buckets for faster access.</param>
        /// <returns>A new instance of the <see cref="System.Buffers.ArrayPool`1"></see> class with the specified configuration.</returns>
        public static ArrayPool<T> Create(int maxArrayLength, int maxArraysPerBucket)
        {
            return new DefaultArrayPool<T>(maxArrayLength, maxArraysPerBucket);
        }

        /// <summary>Retrieves a buffer that is at least the requested length.</summary>
        /// <param name="minimumLength">The minimum length of the array.</param>
        /// <returns>An array of type <see cref="T[]"></see> that is at least <paramref name="minimumLength">minimumLength</paramref> in length.</returns>
        public abstract T[] Rent(int minimumLength);

        /// <summary>Returns an array to the pool that was previously obtained using the <see cref="M:System.Buffers.ArrayPool`1.Rent(System.Int32)"></see> method on the same <see cref="T:System.Buffers.ArrayPool`1"></see> instance.</summary>
        /// <param name="array">A buffer to return to the pool that was previously obtained using the <see cref="M:System.Buffers.ArrayPool`1.Rent(System.Int32)"></see> method.</param>
        /// <param name="clearArray">Indicates whether the contents of the buffer should be cleared before reuse. If <paramref name="clearArray">clearArray</paramref> is set to true, and if the pool will store the buffer to enable subsequent reuse, the <see cref="M:System.Buffers.ArrayPool`1.Return(`0[],System.Boolean)"></see> method will clear the <paramref name="array">array</paramref> of its contents so that a subsequent caller using the <see cref="M:System.Buffers.ArrayPool`1.Rent(System.Int32)"></see> method will not see the content of the previous caller. If <paramref name="clearArray">clearArray</paramref> is set to false or if the pool will release the buffer, the array&amp;#39;s contents are left unchanged.</param>
        public abstract void Return(T[] array, bool clearArray = false);

        /// <summary>Initializes a new instance of the <see cref="T:System.Buffers.ArrayPool`1"></see> class.</summary>
        protected ArrayPool()
        {
        }
    }
}

﻿/*
 * This file is part of the OpenNos Emulator Project. See AUTHORS file for Copyright information
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 */

using OpenNos.Core.ArrayExtensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace OpenNos.Core
{
    namespace ArrayExtensions
    {
        internal static class ArrayExtensions
        {
            #region Methods

            public static void ForEach(this Array array, Action<Array, int[]> action)
            {
                if (array.LongLength == 0)
                {
                    return;
                }
                ArrayTraverse walker = new ArrayTraverse(array);
                do
                {
                    action?.Invoke(array, walker._position);
                }
                while (walker.Step());
            }

            #endregion
        }

        internal class ArrayTraverse
        {
            #region Members

            public readonly int[] _position;

            private readonly int[] _maxLengths;

            #endregion

            #region Instantiation

            public ArrayTraverse(Array array)
            {
                _maxLengths = new int[array.Rank];
                for (int i = 0; i < array.Rank; ++i)
                {
                    _maxLengths[i] = array.GetLength(i) - 1;
                }
                _position = new int[array.Rank];
            }

            #endregion

            #region Methods

            public bool Step()
            {
                for (int i = 0; i < _position.Length; ++i)
                {
                    if (_position[i] < _maxLengths[i])
                    {
                        _position[i]++;
                        for (int j = 0; j < i; j++)
                        {
                            _position[j] = 0;
                        }
                        return true;
                    }
                }
                return false;
            }

            #endregion
        }
    }

    public static class TimeExtensions
    {
        public static DateTime RoundUp(DateTime dt, TimeSpan d) => new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
    }

    namespace ExceptionExtensions
    {
        /// <summary>
        /// Defines a What a Terrible Fault Exception, which should actually never be thrown.
        /// "For those times when:
        /// if (true == false)
        /// {
        ///     Console.WriteLine("Logic is dead!");
        /// }
        /// actually executes.."
        /// </summary>
        public class WTFException : Exception
        {
            public WTFException()
            {
            }

            public WTFException(string message) : base("WTF!!?!??!!1one11! " + message)
            {
            }

            public WTFException(string message, Exception innerException) : base("WTF!!?!??!!1one11! " + message, innerException)
            {
            }

            protected WTFException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        /// <summary>
        /// Defines a CommunicationException thrown mostly by TcpConnections
        /// </summary>
        public class CommunicationException : Exception
        {
            public CommunicationException()
            {
            }

            public CommunicationException(string message) : base(nameof(CommunicationException) + message)
            {
            }

            public CommunicationException(string message, Exception innerException) : base(nameof(CommunicationException) + message, innerException)
            {
            }

            protected CommunicationException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }

    public static class ConcurrentBagExtensions
    {
        #region Methods

        public static void AddRange<T>(this ConcurrentBag<T> bag, List<T> list)
        {
            foreach (T item in list)
            {
                bag.Add(item);
            }
        }

        public static void Clear<T>(this ConcurrentBag<T> bag)
        {
            while (bag.Count > 0)
            {
                bag.TryTake(out T item);
            }
        }

        public static ConcurrentBag<T> Where<T>(this ConcurrentBag<T> bag, Func<T, bool> predicate)
        {
            ConcurrentBag<T> newBag = new ConcurrentBag<T>();
            foreach (T item in bag.ToList().Where(predicate))
            {
                newBag.Add(item);
            }
            return newBag;
        }

        #endregion
    }

    public static class Extension
    {
        #region Methods

        public static string Truncate(this string str, int length) => str.Length > length ? str.Substring(0, length) : str;

        public static bool IsFileLocked(this FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                stream?.Close();
            }
            return false;
        }

        #endregion
    }

    public static class ObjectExtensions
    {
        #region Members

        private static readonly MethodInfo _cloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

        #endregion

        #region Methods

        public static object Copy(this object originalObject) => internalCopy(originalObject, new Dictionary<object, object>(new ReferenceEqualityComparer()));

        public static T Copy<T>(this T original) => (T)Copy((object)original);

        public static bool IsPrimitive(this Type type)
        {
            if (type == typeof(string))
            {
                return true;
            }
            return type.IsValueType & type.IsPrimitive;
        }

        private static void copyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
        {
            foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
            {
                if (filter != null && !filter(fieldInfo))
                {
                    continue;
                }
                if (IsPrimitive(fieldInfo.FieldType))
                {
                    continue;
                }
                object originalFieldValue = fieldInfo.GetValue(originalObject);
                object clonedFieldValue = internalCopy(originalFieldValue, visited);
                fieldInfo.SetValue(cloneObject, clonedFieldValue);
            }
        }

        private static object internalCopy(object originalObject, IDictionary<object, object> visited)
        {
            if (originalObject == null)
            {
                return null;
            }
            Type typeToReflect = originalObject.GetType();
            if (IsPrimitive(typeToReflect))
            {
                return originalObject;
            }
            if (visited.ContainsKey(originalObject))
            {
                return visited[originalObject];
            }
            if (typeof(Delegate).IsAssignableFrom(typeToReflect))
            {
                return null;
            }
            object cloneObject = _cloneMethod.Invoke(originalObject, null);
            if (typeToReflect.IsArray)
            {
                Type arrayType = typeToReflect.GetElementType();
                if (IsPrimitive(arrayType))
                {
                    Array clonedArray = (Array)cloneObject;
                    clonedArray.ForEach((array, indices) => array.SetValue(internalCopy(clonedArray.GetValue(indices), visited), indices));
                }
            }
            visited.Add(originalObject, cloneObject);
            copyFields(originalObject, visited, cloneObject, typeToReflect);
            recursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
            return cloneObject;
        }

        private static void recursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
        {
            if (typeToReflect.BaseType != null)
            {
                recursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
                copyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
            }
        }

        #endregion
    }

    public class ReferenceEqualityComparer : EqualityComparer<object>
    {
        #region Methods

        public override bool Equals(object x, object y) => ReferenceEquals(x, y);

        public override int GetHashCode(object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            return obj.GetHashCode();
        }

        #endregion
    }

    internal static class ConcurrentQueueExtensions
    {
        #region Methods

        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            while (queue.TryDequeue(out T item))
            {
                // do nothing
            }
        }

        #endregion
    }
}
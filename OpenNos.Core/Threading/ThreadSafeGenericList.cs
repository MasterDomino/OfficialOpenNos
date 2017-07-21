/*
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OpenNos.Core
{
    //Definitely not the best approach, but it does what it has to do.
    public class ThreadSafeGenericList<T> : IDisposable
    {
        #region Members

        protected readonly List<T> _list;
        protected readonly ReaderWriterLockSlim Lock;
        private bool _disposed;

        #endregion

        #region Instantiation

        public ThreadSafeGenericList()
        {
            _list = new List<T>();
            Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        }

        #endregion

        #region Properties

        public int Count
        {
            get
            {
                if (!_disposed)
                {
                    Lock.EnterReadLock();
                    try
                    {
                        return _list.Count;
                    }
                    finally
                    {
                        Lock.ExitReadLock();
                    }
                }
                return 0;
            }
        }

        #endregion

        #region Methods

        public void Add(T value)
        {
            if (!_disposed)
            {
                Lock.EnterWriteLock();
                try
                {
                    _list.Add(value);
                }
                finally
                {
                    Lock.ExitWriteLock();
                }
            }
        }

        public void AddRange(List<T> value)
        {
            if (!_disposed)
            {
                Lock.EnterWriteLock();
                try
                {
                    _list.AddRange(value);
                }
                finally
                {
                    Lock.ExitWriteLock();
                }
            }
        }

        public bool Any(Func<T, bool> predicate)
        {
            if (!_disposed)
            {
                Lock.EnterReadLock();
                try
                {
                    return _list.Any(predicate);
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
            return false;
        }

        public bool All(Func<T, bool> predicate)
        {
            if (!_disposed)
            {
                Lock.EnterReadLock();
                try
                {
                    return _list.All(predicate);
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
            return false;
        }

        public int CountLinq(Func<T, bool> predicate)
        {
            if (!_disposed)
            {
                Lock.EnterReadLock();
                try
                {
                    return _list.Count(predicate);
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
            return 0;
        }

        public void CopyTo(T[] grpmembers)
        {
            if (!_disposed)
            {
                Lock.EnterReadLock();
                try
                {
                    _list.CopyTo(grpmembers);
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
        }

        public void Clear()
        {
            if (!_disposed)
            {
                Lock.EnterWriteLock();
                try
                {
                    _list.Clear();
                }
                finally
                {
                    Lock.ExitWriteLock();
                }
            }
        }

        public T ElementAt(int v)
        {
            if (!_disposed)
            {
                Lock.EnterReadLock();
                try
                {
                    return _list[v];
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
            return default(T);
        }

        public T FirstOrDefault()
        {
            if (!_disposed)
            {
                Lock.EnterReadLock();
                try
                {
                    return _list.FirstOrDefault();
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
            return default(T);
        }

        public T FirstOrDefault(Func<T, bool> predicate)
        {
            if (!_disposed)
            {
                Lock.EnterReadLock();
                try
                {
#pragma warning disable RCS1119 // Call 'Find' method instead of 'FirstOrDefault' method.
                    return _list.FirstOrDefault(predicate);
#pragma warning restore RCS1119 // Call 'Find' method instead of 'FirstOrDefault' method.
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
            return default(T);
        }

        public T Find(Predicate<T> predicate)
        {
            if (!_disposed)
            {
                Lock.EnterReadLock();
                try
                {
                    return _list.Find(predicate);
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
            return default(T);
        }

        public void ForEach(Action<T> action)
        {
            if (!_disposed)
            {
                Lock.EnterReadLock();
                try
                {
                    _list.ForEach(action);
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
        }

        public List<T> GetAllItems()
        {
            if (!_disposed)
            {
                Lock.EnterReadLock();
                try
                {
                    return new List<T>(_list);
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
            return new List<T>();
        }

        public void RemoveAll(Predicate<T> match)
        {
            if (!_disposed)
            {
                Lock.EnterWriteLock();
                try
                {
                    _list.RemoveAll(match);
                }
                finally
                {
                    Lock.ExitWriteLock();
                }
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Clear();
                Lock.Dispose();
            }
        }

        #endregion
    }
}
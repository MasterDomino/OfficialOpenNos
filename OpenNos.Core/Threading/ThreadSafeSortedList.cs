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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OpenNos.Core
{
    /// <summary>
    /// This class is used to store key-value based items in a thread safe manner. It uses
    /// System.Collections.Generic.SortedList publicly.
    /// </summary>
    /// <typeparam name="TK">Key type</typeparam>
    /// <typeparam name="TV">Value type</typeparam>
    public class ThreadSafeSortedList<TK, TV> : IDisposable
    {
        #region Members

        /// <summary>
        /// private collection to store items.
        /// </summary>
        private readonly SortedList<TK, TV> _items;

        /// <summary>
        /// Used to synchronize access to Items list.
        /// </summary>
        private readonly ReaderWriterLockSlim _lock;

        private bool _disposed;

        #endregion

        #region Instantiation

        /// <summary>
        /// Creates a new ThreadSafeSortedList object.
        /// </summary>
        public ThreadSafeSortedList()
        {
            _items = new SortedList<TK, TV>();
            _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets count of items in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _items.Count;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets/adds/replaces an item by key.
        /// </summary>
        /// <param name="key">Key to get/set value</param>
        /// <returns>Item associated with this key</returns>
        public TV this[TK key]
        {
            get
            {
                if (!_disposed)
                {
                    _lock.EnterReadLock();
                    try
                    {
                        return _items.ContainsKey(key) ? _items[key] : default;
                    }
                    finally
                    {
                        _lock.ExitReadLock();
                    }
                }
                return default;
            }

            set
            {
                if (!_disposed)
                {
                    _lock.EnterWriteLock();
                    try
                    {
                        _items[key] = value;
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Removes all items from list.
        /// </summary>
        public void ClearAll()
        {
            if (!_disposed)
            {
                _lock.EnterWriteLock();
                try
                {
                    _items.Clear();
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Checks if collection contains spesified key.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True; if collection contains given key</returns>
        public bool ContainsKey(TK key)
        {
            if (!_disposed)
            {
                _lock.EnterReadLock();
                try
                {
                    return _items.ContainsKey(key);
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if collection contains spesified item.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>True; if collection contains given item</returns>
        public bool ContainsValue(TV item)
        {
            if (!_disposed)
            {
                _lock.EnterReadLock();
                try
                {
                    return _items.ContainsValue(item);
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a number that represents how many elements in the specified sequence satisfy a condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>integer number of found elements</returns>
        public int CountLinq(Func<TV, bool> predicate)
        {
            if (!_disposed)
            {
                _lock.EnterReadLock();
                try
                {
                    return _items.Values.Count(predicate);
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
            return 0;
        }

        /// <summary>
        /// Disposes the current object.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Gets all items in collection.
        /// </summary>
        /// <returns>Item list</returns>
        public List<TV> GetAllItems()
        {
            if (!_disposed)
            {
                _lock.EnterReadLock();
                try
                {
                    return new List<TV>(_items.Values);
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
            return new List<TV>();
        }

        /// <summary>
        /// Gets then removes all items in collection.
        /// </summary>
        /// <returns>Item list</returns>
        public List<TV> GetAndClearAllItems()
        {
            if (!_disposed)
            {
                _lock.EnterWriteLock();
                try
                {
                    List<TV> list = new List<TV>(_items.Values);
                    _items.Clear();
                    return list;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
            return new List<TV>();
        }

        /// <summary>
        /// Removes an item from collection.
        /// </summary>
        /// <param name="key">Key of item to remove</param>
        public bool Remove(TK key)
        {
            if (!_disposed)
            {
                _lock.EnterWriteLock();
                try
                {
                    if (!_items.ContainsKey(key))
                    {
                        return false;
                    }

                    _items.Remove(key);
                    return true;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
            return false;
        }

        /// <summary>
        /// returns list based on given predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<TV> Where(Func<TV, bool> predicate)
        {
            if (!_disposed)
            {
                _lock.EnterReadLock();
                try
                {
                    return new List<TV>(_items.Values.Where(predicate));
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
            return new List<TV>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearAll();
                _lock.Dispose();
            }
        }

        #endregion
    }
}
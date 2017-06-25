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

namespace OpenNos.Core
{
    //Definitely not the best approach, but it does what it has to do.
    public class ThreadSafeGenericList<T>
    {
        #region Members

        private readonly List<T> _list;
        private readonly object _sync;

        #endregion

        #region Instantiation

        public ThreadSafeGenericList()
        {
            _list = new List<T>();
            _sync = new object();
        }

        #endregion

        #region Properties

        public int Count
        {
            get
            {
                lock (_sync)
                {
                    return _list.Count;
                }
            }
        }

        #endregion

        #region Methods

        public void Add(T value)
        {
            lock (_sync)
            {
                _list.Add(value);
            }
        }

        public bool Any(Func<T, bool> predicate)
        {
            lock (_sync)
            {
                return _list.Any(predicate);
            }
        }

        public int CountLinq(Func<T, bool> predicate)
        {
            lock (_sync)
            {
                return _list.Count(predicate);
            }
        }

        public void CopyTo(T[] grpmembers)
        {
            _list.CopyTo(grpmembers);
        }

        public void Clear()
        {
            lock (_sync)
            {
                _list.Clear();
            }
        }

        public T ElementAt(int v)
        {
            lock (_sync)
            {
                return _list[v];
            }
        }

        public T FirstOrDefault()
        {
            lock (_sync)
            {
                return _list.FirstOrDefault();
            }
        }

        public T FirstOrDefault(Func<T, bool> predicate)
        {
            lock (_sync)
            {
                return _list.FirstOrDefault(predicate);
            }
        }

        public T Find(Predicate<T> predicate)
        {
            lock (_sync)
            {
                return _list.Find(predicate);
            }
        }

        public void ForEach(Action<T> action)
        {
            lock (_sync)
            {
                _list.ForEach(action);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (_sync)
            {
                return _list.GetEnumerator();
            }
        }

        public void RemoveAll(Predicate<T> match)
        {
            lock (_sync)
            {
                _list.RemoveAll(match);
            }
        }

        public T Single(Func<T, bool> p)
        {
            lock (_sync)
            {
                return _list.Single(p);
            }
        }

        public int Sum(Func<T, int> p)
        {
            lock (_sync)
            {
                return _list.Sum(p);
            }
        }

        public IEnumerable<T> Where(Func<T, bool> p)
        {
            lock (_sync)
            {
                return _list.Where(p);
            }
        }

        #endregion
    }
}
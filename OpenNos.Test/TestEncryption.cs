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

using OpenNos.Core;
using System.Text;

namespace OpenNos.Test
{
    public class TestEncryption : EncryptionBase
    {
        #region Instantiation

        public TestEncryption() : base(true)
        {
        }

        public TestEncryption(bool hasCustomParameter) : base(hasCustomParameter)
        {
        }

        #endregion

        #region Methods

        public override string Decrypt(byte[] data, int sessionId = 0)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetString(data);
        }

        public override byte[] Encrypt(string data)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(data);
        }

        #endregion
    }
}
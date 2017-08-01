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
using System.Text;

namespace OpenNos.Core
{
    public class WorldEncryption : EncryptionBase
    {
        #region Instantiation

        public WorldEncryption() : base(true)
        {
        }

        #endregion

        #region Methods

        public override string Decrypt(byte[] data, int sessionId = 0)
        {
            try
            {
                string decrypt = string.Empty;
                for (int i = 0; i < data.Length; i++)
                {
                    decrypt += Convert.ToChar(data[i] - (0x40 + (byte)sessionId));
                }
                return decrypt == "0\n" ? string.Empty : decrypt;
            }
            catch
            {
                return string.Empty;
            }
        }

        public override byte[] Encrypt(string data)
        {
            byte[] StrBytes = Encoding.Default.GetBytes(data);
            int BytesLength = StrBytes.Length;

            byte[] encryptedData = new byte[BytesLength + (int)Math.Ceiling((decimal)BytesLength / 0x7E) + 1];

            int ii = 0;
            for (int i = 0; i < BytesLength; i++)
            {
                if (i % 0x7E == 0)
                {
                    encryptedData[i + ii] = (byte)(BytesLength - i > 0x7E ? 0x7E : BytesLength - i);
                    ii++;
                }
                encryptedData[i + ii] = (byte)~StrBytes[i];
            }
            encryptedData[encryptedData.Length - 1] = 0xFF;

            return encryptedData;
        }

        #endregion
    }
}
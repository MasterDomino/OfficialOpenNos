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

using OpenNos.Core;
using OpenNos.DAL;
using OpenNos.Data;
using OpenNos.Domain;
using OpenNos.Master.Library.Data;
using OpenNos.Master.Library.Interface;
using OpenNos.SCS.Communication.Scs.Communication.EndPoints.Tcp;
using OpenNos.SCS.Communication.ScsServices.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reactive.Linq;

namespace OpenNos.Master.Server
{
    internal class MailService : ScsService, IMailService
    {
        public bool Authenticate(string authKey)
        {
            if (string.IsNullOrWhiteSpace(authKey))
            {
                return false;
            }

            if (authKey == ConfigurationManager.AppSettings["MasterAuthKey"])
            {
                MSManager.Instance.AuthentificatedClients.Add(CurrentClient.ClientId);
                return true;
            }

            return false;
        }

        public void SendMail(Mail mail)
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return;
            }

            MailDTO mailDTO = new Data.MailDTO
            {
                AttachmentAmount = mail.AttachmentAmount,
                AttachmentRarity = mail.AttachmentRarity,
                AttachmentUpgrade = mail.AttachmentUpgrade,
                AttachmentVNum = mail.AttachmentVNum,
                Date = mail.Date,
                EqPacket = mail.EqPacket,
                IsOpened = mail.IsOpened,
                IsSenderCopy = mail.IsSenderCopy,
                MailId = mail.MailId,
                Message = mail.Message,
                ReceiverId = mail.ReceiverId,
                SenderClass = mail.SenderClass,
                SenderGender = mail.SenderGender,
                SenderHairColor = mail.SenderHairColor,
                SenderHairStyle = mail.SenderHairStyle,
                SenderId = mail.SenderId,
                SenderMorphId = mail.SenderMorphId,
                Title = mail.Title
            };

            DAOFactory.MailDAO.InsertOrUpdate(ref mailDTO);
            mail.MailId = mailDTO.MailId;

            AccountConnection account = MSManager.Instance.ConnectedAccounts.Find(a => a.CharacterId.Equals(mail.ReceiverId));
            if (account?.ConnectedWorld != null)
            {
                account.ConnectedWorld.ServiceClient.GetClientProxy<IMailClient>().MailSent(mail);
            }
            if (mail.IsSenderCopy)
            {

                account = MSManager.Instance.ConnectedAccounts.Find(a => a.CharacterId.Equals(mail.SenderId));
                if (account?.ConnectedWorld != null)
                {
                    account.ConnectedWorld.ServiceClient.GetClientProxy<IMailClient>().MailSent(mail);
                }
            }
        }
    }
}

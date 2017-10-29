using OpenNos.Master.Library.Data;
using OpenNos.Master.Library.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.Master.Library.Client
{
    internal  class MailClient : IMailClient
    {
        public void MailSent(Mail mail) => Task.Run(() => MailServiceClient.Instance.OnMailSent(mail));

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Postcore.AdApi.Shared.Messages
{
    public class ConfirmAdMessage
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
    }
}
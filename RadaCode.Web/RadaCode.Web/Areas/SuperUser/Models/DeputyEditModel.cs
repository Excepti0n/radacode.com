using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace putaty.web.Areas.SuperUser.Models
{
    public class DeputyEditModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string Patronimic { get; set; }

        public string LastName { get; set; }

        public bool HasImage { get; set; }

        public string Bio { get; set; }

        public string Gender { get; set; }
    }
}
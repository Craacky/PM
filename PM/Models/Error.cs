using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM.Models
{
    public class Error
    {
        public string TypeError { get; set; }

        public string ProductCode { get; set; }

        public List<string> BoxCodes { get; set; } = new List<string>();
        public List<string> PalletCodes { get; set; } = new List<string>();

    }
}

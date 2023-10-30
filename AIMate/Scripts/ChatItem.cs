using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMate.Scripts
{
    internal class ChatItem
    {
        public string Message { get; set; }
        public double Height { get; set; } = 40; // Set the default height to 40

        public bool MarkedForDeletion { get; set; } // new property for deletion status
    }
}

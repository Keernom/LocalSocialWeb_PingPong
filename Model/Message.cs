using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovik_Kocherzhenko.Model
{
    public class Message
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int Sender { get; set; }
        public int Getter { get; set; }
        public string Text { get; set; }
    }
}

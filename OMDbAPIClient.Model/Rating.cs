using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMDbAPIClient.Model
{
    public class Rating
    {
        public string Source { get; set; }
        public string Value { get; set; }


        public override string ToString()
        {
            return Source + ": " + Value; 
        }
    }
}

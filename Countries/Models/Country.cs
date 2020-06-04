using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Countries.Models
{
    public class Country
    {
        public string name { get; set; }        
        public string alpha2Code { get; set; }
        public string alpha3Code { get; set; }        
        public string capital { get; set; }        
        public string region { get; set; }
        public string subRegion { get; set; }
        public string population { get; set; }        
        public string demonym { get; set; }
        public string area { get; set; }
        public string gini { get; set; }        
        public string nativeName { get; set; }
        public string numericCode { get; set; }        
        public string flag { get; set; }
        public string flagPNG { get; set; }
        public string cioc { get; set; }

        //public override string ToString()
        //{
        //    return $"{name}";
        //}
    }
}

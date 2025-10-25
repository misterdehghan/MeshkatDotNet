using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Models
{
    public class option
    {
        public option(string tex, int val)
        {
            this.value = val;
            this.text = tex;
        }
        public option()
        {

        }
        public int value { get; set; }
        public string text { get; set; }
    }
    public class optionAllDarajeh
    {
        public optionAllDarajeh(string tex, int val , int _type)
        {
            this.value = val;
            this.text = tex;
            this.drtype = _type;
        }
        public optionAllDarajeh()
        {

        }
        public int drtype { get; set; }
        public int value { get; set; }
        public string text { get; set; }
    }
    public class Listoption
    {
        public Listoption()
        {

        }
        public Listoption(List<option> options)
        {
            this.lstoption = options;
        }
        public List<option> lstoption { get; set; } = new List<option>();
    }

}

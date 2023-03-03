using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    

    internal class Car
    {
        public delegate void ChangeMakeEventHandler(object sender);

        string _make;

        public event ChangeMakeEventHandler OnMakeChange;

        public string Make
        {
            get { return _make; }
            set 
            {
                _make = value;
                RaiseMakeChangeEvent();
            }
        }
      
        internal void RaiseMakeChangeEvent()
        { 
            if(OnMakeChange != null)
                OnMakeChange(this);
        }

        public string Model { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        
        private string manufacturer;

        public string GetMakeAndModel()
        {
            return Make + " " + Model;
        }

        public override string ToString()
        {
            return string.Format("{0} --- {1} --- {2} --- {3}\n", 
                Make, Model, Name, Year.ToString());


        }
    }
}

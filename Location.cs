using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    internal class Location
    {

        public float Latitude { set; get; }
        public float Longitude { set; get; }

        public Location()
        {
            Latitude = 0;
            Longitude = 0;
        }
        public void SetLocation(float lat, float longi)
        {
            Latitude = lat;
            Longitude = longi;
        }
    }
}

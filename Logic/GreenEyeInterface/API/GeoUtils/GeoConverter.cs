using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenEyeAPI.Core.GeoUtils
{
    public class GeoConverter
    {
        public enum EnumMapDatum
        {
            WGS_84 = 0,
            NAD_83 = 1,
            GRS_80 = 2,
            WGS_72 = 3,
            Australian_1985 = 4,
            Krasovsly_1940 = 5,
            North_American_1927 = 6,
            International_1924 = 7,
            Hayford_1909 = 8,
            Clarke_1880 = 9,
            Clarke_1866 = 10,
            Airy_1830 = 11,
            Bessel_1841 = 12,
            Everest_1830 = 13
        }

        ///
        /// convert a position in decimal degrees to the various other formats. does 
        /// input validity checks prior to calling conversion routines...
        ///
        public void GeoToUTM(double pLat, double pLon, EnumMapDatum pMapDatum, out double pEast, out double pNorth, out int pZone, out string pHemisphare) {
            if (pLat > 90 || pLat < -90) 
                throw new Exception("Latitude must be between -90 and 90");

            if (pLon > 180 || pLon < -180) 
                throw new Exception("Longitude must be between -180 and 180");

            //
            // 0 is N or W hemisphere, 1 is S or E
            //
            UTM utmconv = new UTM();
            utmconv.SetDatum((int)pMapDatum);
            utmconv.LatLngToUtm(pLat, pLon, out pEast, out pNorth, out pZone, out pHemisphare);
        }

        ///
        /// convert a set of UTM coordinates into the various other formats.  checks input for validity 
        /// before calling the various conversion routines.
        ///
        public void UTMToGeo(double pEast, double pNorth, int pZone, string pHemisphare, out double pLat, out double pLon) {
            if (pZone < 1 || pZone > 60) 
                throw new Exception("Longitude zone must be between 1 and 60");

            if (pNorth < 0 || pNorth > 10000000) 
                throw new Exception("Northing must be between 0 and 10000000");


            if (pEast < 160000 || pEast > 834000)
                throw new Exception("Easting coordinate crosses zone boundries, results should be used with caution");

            UTM utmconv = new UTM();
            utmconv.SetDatum(pHemisphare == "S" ? 1 : 0);
            utmconv.UtmToLatLng(pEast, pNorth, pZone, pHemisphare, out pLat, out pLon);    // get lat/lon for this set of coordinates
        }
    }
}

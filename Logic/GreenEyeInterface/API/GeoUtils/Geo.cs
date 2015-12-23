using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenEyeAPI.Core.GeoUtils
{
    public class Geo
    {
        private double _lat;
        private double _lon;
        private double _radius;

        /**
         * Creates a point on the earth's surface at the supplied latitude / longitude
         *
         * @constructor
         * @param {Number} lat: latitude in numeric degrees
         * @param {Number} lon: longitude in numeric degrees
         * @param {Number} [rad=6371]: radius of earth if different value is required from standard 6,371km
         */
        public Geo(double lat, double lon, double rad) {
            Init(lat, lon, rad);
        }

        public Geo(double lat, double lon) {
            Init(lat, lon, 6371);
        }

        private void Init (double lat, double lon, double rad) {
            //if (typeof (rad) == 'undefined') rad = 6371;  // earth's mean radius in km
            // only accept numbers or valid numeric strings
            this._lat = lat;
            this._lon = lon;
            this._radius = rad;
        } 


        /**
         * Returns the distance from this point to the supplied point, in km 
         * (using Haversine formula)
         *
         * from: Haversine formula - R. W. Sinnott, "Virtues of the Haversine",
         *       Sky and Telescope, vol 68, no 2, 1984
         *
         * @param   {LatLon} point: Latitude/longitude of destination point
         * @param   {Number} [precision=4]: no of significant digits to use for returned value
         * @returns {Number} Distance in km between this point and destination point
         */
        public double distanceTo (double pLat, double pLon) {
            return distanceTo(pLat, pLon, 4);
        }

        public double distanceTo (double pLat, double pLon, int precision) {
            double R = this._radius;
            double lat1 = toRad(this._lat), lon1 = toRad(this._lon);
            double lat2 = toRad(pLat);
            double lon2 = toRad(pLon);
            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;
            return double.Parse(toPrecisionFixed(d, precision));
        }


        /**
         * Returns the (initial) bearing from this point to the supplied point, in degrees
         *   see http://williams.best.vwh.net/avform.htm#Crs
         *
         * @param   {LatLon} point: Latitude/longitude of destination point
         * @returns {Number} Initial bearing in degrees from North
         */
        public double bearingTo(double pLat, double pLon)
        {
            double lat1 = toRad(this._lat);
            double lat2 = toRad(pLat);
            double dLon = toRad(pLon - this._lon);

            var y = Math.Sin(dLon) * Math.Cos(lat2);
            var x = Math.Cos(lat1) * Math.Sin(lat2) -
                    Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            var brng = Math.Atan2(y, x);

            return (toDeg(brng) + 360) % 360;
        }


        /**
         * Returns final bearing arriving at supplied destination point from this point; the final bearing 
         * will differ from the initial bearing by varying degrees according to distance and latitude
         *
         * @param   {LatLon} point: Latitude/longitude of destination point
         * @returns {Number} Final bearing in degrees from North
         */
        public double finalBearingTo(double pLat, double pLon)
        {
            // get initial bearing from supplied point back to this point...
            double lat1 = toRad(pLat);
            double lat2 = toRad(this._lat);
            double dLon = toRad(this._lon - pLon);

            var y = Math.Sin(dLon) * Math.Cos(lat2);
            var x = Math.Cos(lat1) * Math.Sin(lat2) -
                    Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            var brng = Math.Atan2(y, x);

            // ... & reverse it by adding 180°
            return (toDeg(brng) + 180) % 360;
        }


        /**
         * Returns the midpoint between this point and the supplied point.
         *   see http://mathforum.org/library/drmath/view/51822.html for derivation
         *
         * @param   {LatLon} point: Latitude/longitude of destination point
         * @returns {LatLon} Midpoint between this point and the supplied point
         */
        public void midpointTo(double pLat, double pLon, out double pMidPointLat, out double pMidPointLon)
        {
            double lat1 = toRad(this._lat);
            double lon1 = toRad(this._lon);
            double lat2 = toRad(pLat);
            double dLon = toRad(pLon - this._lon);

            var Bx = Math.Cos(lat2) * Math.Cos(dLon);
            var By = Math.Cos(lat2) * Math.Sin(dLon);

            double lat3 = Math.Atan2(Math.Sin(lat1) + Math.Sin(lat2),
                              Math.Sqrt((Math.Cos(lat1) + Bx) * (Math.Cos(lat1) + Bx) + By * By));
            double lon3 = lon1 + Math.Atan2(By, Math.Cos(lat1) + Bx);
            lon3 = (lon3 + 3 * Math.PI) % (2 * Math.PI) - Math.PI;  // normalise to -180..+180º

            pMidPointLat = toDeg(lat3);
            pMidPointLon = toDeg(lon3);
        }


        /**
         * Returns the destination point from this point having travelled the given distance (in km) on the 
         * given initial bearing (bearing may vary before destination is reached)
         *
         *   see http://williams.best.vwh.net/avform.htm#LL
         *
         * @param   {Number} brng: Initial bearing in degrees
         * @param   {Number} dist: Distance in km
         * @returns {LatLon} Destination point
         */
        public void destinationPoint(double brng, double dist, out double pLat, out double pLon) 
        {
            dist = dist / this._radius;  // convert dist to angular distance in radians
            brng = toRad(brng);  // 
            double lat1 = toRad(this._lat);
            double lon1 = toRad(this._lon);

            var lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(dist) +
                                  Math.Cos(lat1) * Math.Sin(dist) * Math.Cos(brng));
            var lon2 = lon1 + Math.Atan2(Math.Sin(brng) * Math.Sin(dist) * Math.Cos(lat1),
                                         Math.Cos(dist) - Math.Sin(lat1) * Math.Sin(lat2));
            lon2 = (lon2 + 3 * Math.PI) % (2 * Math.PI) - Math.PI;  // normalise to -180..+180º

            pLat = toDeg(lat2);
            pLon = toDeg(lon2);
        }


        /**
         * Returns the point of intersection of two paths defined by point and bearing
         *
         *   see http://williams.best.vwh.net/avform.htm#Intersection
         *
         * @param   {LatLon} p1: First point
         * @param   {Number} brng1: Initial bearing from first point
         * @param   {LatLon} p2: Second point
         * @param   {Number} brng2: Initial bearing from second point
         * @returns {LatLon} Destination point (null if no unique intersection defined)
         */
        public void intersection(double pP1Lat, double pP1Lon, double pP1Bearing, double pP2Lat, double pP2Lon, double pP2Bearing, out double pLat, out double pLon)
        {
            double lat1 = toRad(pP1Lat);
            double lon1 = toRad(pP1Lon);
            double lat2 = toRad(pP2Lat);
            double lon2 = toRad(pP2Lon);
            double brng13 = toRad(pP1Bearing);
            double brng23 = toRad(pP2Bearing);
            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;
            double brng12 = 0;
            double brng21 = 0;

            pLat = 0;
            pLon = 0;

            double dist12 = 2 * Math.Asin(Math.Sqrt(Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2)));
            
            if (dist12 == 0) 
                return;

            // initial/final bearings between points
            double brngA = Math.Acos((Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(dist12)) /
              (Math.Sin(dist12) * Math.Cos(lat1)));
            double brngB = Math.Acos((Math.Sin(lat1) - Math.Sin(lat2) * Math.Cos(dist12)) /
              (Math.Sin(dist12) * Math.Cos(lat2)));

            if (Math.Sin(lon2 - lon1) > 0) {
                brng12 = brngA;
                brng21 = 2 * Math.PI - brngB;
            } else {
                brng12 = 2 * Math.PI - brngA;
                brng21 = brngB;
            }

            double alpha1 = (brng13 - brng12 + Math.PI) % (2 * Math.PI) - Math.PI;  // angle 2-1-3
            double alpha2 = (brng21 - brng23 + Math.PI) % (2 * Math.PI) - Math.PI;  // angle 1-2-3

            if (Math.Sin(alpha1) == 0 && Math.Sin(alpha2) == 0) return;  // infinite intersections
            if (Math.Sin(alpha1) * Math.Sin(alpha2) < 0) return;       // ambiguous intersection

            //alpha1 = Math.abs(alpha1);
            //alpha2 = Math.abs(alpha2);
            // ... Ed Williams takes abs of alpha1/alpha2, but seems to break calculation?

            double alpha3 = Math.Acos(-Math.Cos(alpha1) * Math.Cos(alpha2) +
                                 Math.Sin(alpha1) * Math.Sin(alpha2) * Math.Cos(dist12));
            double dist13 = Math.Atan2(Math.Sin(dist12) * Math.Sin(alpha1) * Math.Sin(alpha2),
                                 Math.Cos(alpha2) + Math.Cos(alpha1) * Math.Cos(alpha3));
            double lat3 = Math.Asin(Math.Sin(lat1) * Math.Cos(dist13) +
                              Math.Cos(lat1) * Math.Sin(dist13) * Math.Cos(brng13));
            double dLon13 = Math.Atan2(Math.Sin(brng13) * Math.Sin(dist13) * Math.Cos(lat1),
                                 Math.Cos(dist13) - Math.Sin(lat1) * Math.Sin(lat3));
            double lon3 = lon1 + dLon13;
            lon3 = (lon3 + 3 * Math.PI) % (2 * Math.PI) - Math.PI;  // normalise to -180..+180º

            pLat = toDeg(lat3);
            pLon = toDeg(lon3);
        }


        /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  */

        /**
         * Returns the distance from this point to the supplied point, in km, travelling along a rhumb line
         *
         *   see http://williams.best.vwh.net/avform.htm#Rhumb
         *
         * @param   {LatLon} point: Latitude/longitude of destination point
         * @returns {Number} Distance in km between this point and destination point
         */
        public double rhumbDistanceTo(double pLat, double pLot)
        {
            double R = this._radius;
            double lat1 = toRad(this._lat);
            double lat2 = toRad(pLat);
            double dLat = toRad(pLat - this._lat);
            double dLon = toRad(Math.Abs(pLot - this._lon));

            double dPhi = Math.Log(Math.Tan(lat2 / 2 + Math.PI / 4) / Math.Tan(lat1 / 2 + Math.PI / 4));
            double q = (!double.IsInfinity(dLat / dPhi)) ? dLat / dPhi : Math.Cos(lat1);  // E-W line gives dPhi=0

            // if dLon over 180° take shorter rhumb across anti-meridian:
            if (Math.Abs(dLon) > Math.PI) {
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            }

            var dist = Math.Sqrt(dLat * dLat + q * q * dLon * dLon) * R;

            return double.Parse(toPrecisionFixed(dist, 4));  // 4 sig figs reflects typical 0.3% accuracy of spherical model
        }

        /**
         * Returns the bearing from this point to the supplied point along a rhumb line, in degrees
         *
         * @param   {LatLon} point: Latitude/longitude of destination point
         * @returns {Number} Bearing in degrees from North
         */
        public double rhumbBearingTo(double pLat, double pLot)
        {
            double lat1 = toRad(this._lat), lat2 = toRad(pLat);
            double dLon = toRad(pLot - this._lon);

            double dPhi = Math.Log(Math.Tan(lat2 / 2 + Math.PI / 4) / Math.Tan(lat1 / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI) dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            double brng = Math.Atan2(dLon, dPhi);

            return (toDeg(brng) + 360) % 360;
        }

        /**
         * Returns the destination point from this point having travelled the given distance (in km) on the 
         * given bearing along a rhumb line
         *
         * @param   {Number} brng: Bearing in degrees from North
         * @param   {Number} dist: Distance in km
         * @returns {LatLon} Destination point
         */
        public void rhumbDestinationPoint(double brng, double dist, out double pDestLat, out double pDestLon)
        {
            double R = this._radius;
            double d = dist / R;  // d = angular distance covered on earth’s surface
            double lat1 = toRad(this._lat), lon1 = toRad(this._lon);
            brng = toRad(brng);

            var dLat = d * Math.Cos(brng);
            // nasty kludge to overcome ill-conditioned results around parallels of latitude:
            if (Math.Abs(dLat) < 1e-10) dLat = 0; // dLat < 1 mm

            var lat2 = lat1 + dLat;
            var dPhi = Math.Log(Math.Tan(lat2 / 2 + Math.PI / 4) / Math.Tan(lat1 / 2 + Math.PI / 4));
            var q = (!double.IsInfinity(dLat / dPhi)) ? dLat / dPhi : Math.Cos(lat1);  // E-W line gives dPhi=0
            var dLon = d * Math.Sin(brng) / q;

            // check for some daft bugger going past the pole, normalise latitude if so
            if (Math.Abs(lat2) > Math.PI / 2) lat2 = lat2 > 0 ? Math.PI - lat2 : -Math.PI - lat2;

            double lon2 = (lon1 + dLon + 3 * Math.PI) % (2 * Math.PI) - Math.PI;

            pDestLat = toDeg(lat2);
            pDestLon = toDeg(lon2);
        }

        /**
         * Returns the loxodromic midpoint (along a rhumb line) between this point and the supplied point.
         *   see http://mathforum.org/kb/message.jspa?messageID=148837
         *
         * @param   {LatLon} point: Latitude/longitude of destination point
         * @returns {LatLon} Midpoint between this point and the supplied point
         */
        public void rhumbMidpointTo(double pLat, double pLon, out double pMidPointLat, out double pMidPointLon) {
            double lat1 = toRad(this._lat);
            double lon1 = toRad(this._lon);
            double lat2 = toRad(pLat);
            double lon2 = toRad(pLon);

            if (Math.Abs(lon2 - lon1) > Math.PI) lon1 += 2 * Math.PI; // crossing anti-meridian

            var lat3 = (lat1 + lat2) / 2;
            var f1 = Math.Tan(Math.PI / 4 + lat1 / 2);
            var f2 = Math.Tan(Math.PI / 4 + lat2 / 2);
            var f3 = Math.Tan(Math.PI / 4 + lat3 / 2);
            var lon3 = ((lon2 - lon1) * Math.Log(f3) + lon1 * Math.Log(f2) - lon2 * Math.Log(f1)) / Math.Log(f2 / f1);

            if (double.IsInfinity(lon3)) lon3 = (lon1 + lon2) / 2; // parallel of latitude

            lon3 = (lon3 + 3 * Math.PI) % (2 * Math.PI) - Math.PI;  // normalise to -180..+180º

            pMidPointLat = toDeg(lat3);
            pMidPointLon = toDeg(lon3);
        }


        /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  */

        // ---- extend Number object with methods for converting degrees/radians

        /** Converts numeric degrees to radians */
        private double toRad(double pNumber)
        {
            return pNumber * Math.PI / 180;
        }

        /** Converts radians to numeric (signed) degrees */
        private double toDeg(double pNumber)
        {
            return pNumber * 180 / Math.PI;
        }

        /** 
         * Formats the significant digits of a number, using only fixed-point notation (no exponential)
         * 
         * @param   {Number} precision: Number of significant digits to appear in the returned string
         * @returns {String} A string representation of number which contains precision significant digits
         */
        private string toPrecisionFixed (double pNumber, int pPrecision)
        {
            return pNumber.ToString("N" + pPrecision);
        }


    }
}

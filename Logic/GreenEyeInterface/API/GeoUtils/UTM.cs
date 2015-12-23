using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenEyeAPI.Core.GeoUtils
{
    public class UTM
    {
        internal class Datum
        {
            public double eqRad;
            public double flat;
        }

        //
        // constants taken from or calculated from the datum
        //
        private double a = 0;   // equatorial radius in meters
        private double f = 0;   // polar flattening
        private double b = 0;   // polar radius in meters
        private double e = 0;   // eccentricity
        private double e0 = 0;  // e'


        //
        // constants used in calculations
        //
        private double k = 1;
        private double k0 = 0.9996;
        private double drad = Math.PI / 180;

        private string digraphLettersE = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        private string digraphLettersN = "ABCDEFGHJKLMNPQRSTUV";
        private string digraphLettersAll = "ABCDEFGHJKLMNPQRSTUVABCDEFGHJKLMNPQRSTUVABCDEFGHJKLMNPQRSTUVABCDEFGHJKLMNPQRSTUVABCDEFGHJKLMNPQRSTUVABCDEFGHJKLMNPQRSTUVABCDEFGHJKLMNPQRSTUVABCDEFGHJKLMNPQRSTUVABCDEFGHJKLMNPQRSTUVABCDEFGHJKLMNPQRSTUV";
        private Datum[] datumTable = {
            new Datum () { eqRad = 6378137.0, flat = 298.2572236 },    // WGS 84
            new Datum () { eqRad = 6378137.0, flat = 298.2572236 },    // NAD 83
            new Datum () { eqRad = 6378137.0, flat = 298.2572215 },    // GRS 80
            new Datum () { eqRad = 6378135.0, flat = 298.2597208 },    // WGS 72
            new Datum () { eqRad = 6378160.0, flat = 298.2497323 },    // Austrailian 1965
            new Datum () { eqRad = 6378245.0, flat = 298.2997381 },    // Krasovsky 1940
            new Datum () { eqRad = 6378206.4, flat = 294.9786982 },    // North American 1927
            new Datum () { eqRad = 6378388.0, flat = 296.9993621 },    // International 1924
            new Datum () { eqRad = 6378388.0, flat = 296.9993621 },    // Hayford 1909
            new Datum () { eqRad = 6378249.1, flat = 293.4660167 },    // Clarke 1880
            new Datum () { eqRad = 6378206.4, flat = 294.9786982 },    // Clarke 1866
            new Datum () { eqRad = 6377563.4, flat = 299.3247788 },    // Airy 1830
            new Datum () { eqRad = 6377397.2, flat = 299.1527052 },    // Bessel 1841
            new Datum () { eqRad = 6377276.3, flat = 300.8021499 }     // Everest 1830
        };

        ///
        /// calculate constants used for doing conversions using a given map datum
        ///
        public void SetDatum(int pIndex) {
            Datum datum = datumTable[pIndex];
            this.a = datum.eqRad;
            this.f = 1 / datum.flat;
            this.b = this.a * (1 - this.f);   // polar radius
            this.e = Math.Sqrt(1 - Math.Pow(this.b, 2) / Math.Pow(this.a, 2));
            this.e0 = this.e / Math.Sqrt(1 - Math.Pow(this.e, 1));
        }

        ///
        /// given a lat/lng pair, returns both global UTM and NATO UTM in the following form:
        /// utm:
        /// { 
        ///     global: { northing: n, easting: e, zone: z, southern: x },
        ///     nato: { northing: n, easting: e, latzone: z0, lngzone: z1, digraph: xx }
        /// } 
        ///
        /// this function assumes that all data validation has been performed prior to calling
        /// it.
        ///
        public void LatLngToUtm (double pLat, double pLngd, out double pEast, out double pNorth, out int pZone, out string pHemisphare) {
            double phi = pLat * this.drad;                              // convert latitude to radians
            double lng = pLngd * this.drad;                             // convert longitude to radians
            double utmz = 1 + Math.Floor((pLngd + 180) / 6);            // longitude to utm zone
            double zcm = 3 + 6 * (utmz - 1) - 180;                     // central meridian of a zone
            double latz = 0;                                           // this gives us zone A-B for below 80S
            double esq = (1 - (this.b / this.a) * (this.b / this.a));
            double e0sq = this.e * this.e / (1 - Math.Pow(this.e, 2));
            double M = 0;

            // convert latitude to latitude zone for nato
            if (pLat > -80 && pLat < 72) {
                latz = Math.Floor((pLat + 80) / 8) + 2;      // zones C-W in this range
            } if (pLat > 72 && pLat < 84) {
                latz = 21;                                  // zone X
            } else if (pLat > 84) {
                latz = 23;                                  // zone Y-Z
            }

            var N = this.a / Math.Sqrt(1 - Math.Pow(this.e * Math.Sin(phi), 2));
            var T = Math.Pow(Math.Tan(phi), 2);
            var C = e0sq * Math.Pow(Math.Cos(phi), 2);
            var A = (pLngd - zcm) * this.drad * Math.Cos(phi);

            // calculate M (USGS style)
            M = phi * (1F - esq * (1F / 4F + esq * (3F / 64F + 5F * esq / 256F)));
            M = M - Math.Sin(2F * phi) * (esq * (3F / 8F + esq * (3F / 32F + 45F * esq / 1024F)));
            M = M + Math.Sin(4F * phi) * (esq * esq * (15F / 256F + esq * 45F / 1024F));
            M = M - Math.Sin(6F * phi) * (esq * esq * esq * (35F / 3072F));
            M = M * this.a;                                      //Arc length along standard meridian

            double M0 = 0;                                         // if another point of origin is used than the equator

            // now we are ready to calculate the UTM values...
            // first the easting
            var x = this.k0 * N * A * (1F + A * A * ((1F - T + C) / 6F + A * A * (5F - 18F * T + T * T + 72F * C - 58F * e0sq) / 120F)); //Easting relative to CM
            x = x + 500000; // standard easting

            // now the northing
            var y = this.k0 * (M - M0 + N * Math.Tan(phi) * (A * A * (1F / 2F + A * A * ((5F - T + 9F * C + 4F * C * C) / 24F + A * A * (61F - 58F * T + T * T + 600F * C - 330F * e0sq) / 720F))));    // first from the equator
            var yg = y + 10000000;  //yg = y global, from S. Pole
            if (y < 0) {
                y = 10000000 + y;   // add in false northing if south of the equator
            }

            pEast = Math.Round(10*(x))/10;
            pNorth = Math.Round(10*y)/10;
            pZone = (int)utmz;
            pHemisphare = ((phi < 0) == true) ? "S" : "N";
        }

        ///
        /// convert a set of global UTM coordinates to lat/lng returned as follows
        ///
        /// { lat: y, lng: x }
        ///
        /// inputs:
        ///     x: easting
        ///     y: northing
        ///     utmz: utm zone
        ///     southern: bool indicating coords are in southern hemisphere
        ///
        public void UtmToLatLng (double pEast, double pNorth, int pZone, string pSouthern, out double pLat, out double pLon) {
            double esq = (1F - (this.b / this.a) * (this.b / this.a));
            double e0sq = this.e * this.e / (1F - Math.Pow(this.e, 2F));
            double zcm = 3F + 6F * (pZone - 1F) - 180F;                         // Central meridian of zone
            double e1 = (1F - Math.Sqrt(1F - Math.Pow(this.e, 2F))) / (1F + Math.Sqrt(1F - Math.Pow(this.e, 2F)));
            double M0 = 0;
            double M = 0;

            if (pSouthern == "N")
                M = M0 + pNorth / this.k0;    // Arc length along standard meridian. 
            else
                M = M0 + (pNorth - 10000000) / this.k;

            var mu = M / (this.a * (1F - esq * (1F / 4F + esq * (3F / 64F + 5F * esq / 256F))));
            var phi1 = mu + e1 * (3F / 2F - 27F * e1 * e1 / 32F) * Math.Sin(2F * mu) + e1 * e1 * (21F / 16F - 55F * e1 * e1 / 32F) * Math.Sin(4F * mu);   //Footprint Latitude
            phi1 = phi1 + e1 * e1 * e1 * (Math.Sin(6F * mu) * 151F / 96F + e1 * Math.Sin(8F * mu) * 1097F / 512F);
            var C1 = e0sq * Math.Pow(Math.Cos(phi1), 2F);
            var T1 = Math.Pow(Math.Tan(phi1), 2F);
            var N1 = this.a / Math.Sqrt(1F - Math.Pow(this.e * Math.Sin(phi1), 2F));
            var R1 = N1 * (1F - Math.Pow(this.e, 2F)) / (1F - Math.Pow(this.e * Math.Sin(phi1), 2F));
            var D = (pEast - 500000F) / (N1 * this.k0);
            var phi = (D * D) * (1 / 2F - D * D * (5F + 3F * T1 + 10F * C1 - 4F * C1 * C1 - 9F * e0sq) / 24F);
            phi = phi + Math.Pow(D, 6) * (61F + 90F * T1 + 298F * C1 + 45F * T1 * T1 - 252F * e0sq - 3F * C1 * C1) / 720F;
            phi = phi1 - (N1 * Math.Tan(phi1) / R1) * phi;

            var lat = Math.Floor(1000000 * phi / this.drad) / 1000000;
            var lng = D * (1F + D * D * ((-1F - 2F * T1 - C1) / 6F + D * D * (5F - 2F * C1 + 28F * T1 - 3F * C1 * C1 + 8F * e0sq + 24F * T1 * T1) / 120F)) / Math.Cos(phi1);
            //lng = lngd = zcm + lng / this.drad;
            lng = zcm + lng / this.drad;

            pLat = lat;
            pLon = lng;
        }

    }
}

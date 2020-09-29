//-----------------------------------------------------------------------
// <copyright file="ReverseGeocodeResource.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Mapbox.Geocoding
{
    using Mapbox.Utils;
    using System.Collections.Generic;
    using System.Runtime.Remoting.Contexts;

    /// <summary> A reverse geocode request. </summary>
    public sealed class GetRubbishLocation : GeocodeResource<Vector2d>
    {
        // Required
        private Vector2d query;

        /// <summary> Initializes a new instance of the <see cref="ReverseGeocodeResource" /> class.</summary>
        /// <param name="query"> Location to reverse geocode. </param>
        public GetRubbishLocation(Vector2d query)
        {
            this.Query = query;
        }

        /// <summary> Gets or sets the location. </summary>
        public override Vector2d Query
        {
            get
            {
                return this.query;
            }

            set
            {
                this.query = value;
            }
        }

        /// <summary> Builds a complete reverse geocode URL string. </summary>
        /// <returns> A complete, valid reverse geocode URL string. </returns>
        public override string GetUrl()
        {
            Dictionary<string, string> opts = new Dictionary<string, string>();

            if (this.Types != null)
            {
                opts.Add("types", GetUrlQueryFromArray(this.Types));
            }

            return Constants.BaseAPI +
                            this.ApiEndpoint +
                            this.Mode +
                            this.Query.ToString() +
                            ".json" +
                            EncodeQueryString(opts) +
                            this.MyTokenReverse;
        }
    }

    /*---- URL Result ----*/

    [System.Serializable]
    public class MyResult
    {
        public string type;
        public List<double> query;
        public List<Features> features;
        public string attribution;
    }

    [System.Serializable]
    public class Features
    {
        public string id;
        public string type;
        public List<string> place_type;
        public int relevance;
        public List<Properties> properties;
        public string text;
        public string place_name;
        public List<double> bbox;
        public List<double> center;
        public List<MyGeometry> geometry;
        public List<Context> context;
    }

    [System.Serializable]
    public class Properties
    {
        public string wikidata;
        public string short_code;
    }

    [System.Serializable]
    public class MyGeometry
    {
        public string type;
        public List<double> coordinates;
    }
}
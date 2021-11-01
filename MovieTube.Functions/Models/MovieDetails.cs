using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTube.Functions.Models
{
    public class MovieDetails
    {
        #region Fields
        private string _poster;
		private string _banner;
		private string _releaseDate;
		private string _trailerPath;
		private string _moviePath;
		private string _rate;
		private string _description;
		private string _name;
		private int _id;
		#endregion


		[JsonProperty(PropertyName = "id")]
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		[JsonProperty(PropertyName = "name")]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		[JsonProperty(PropertyName = "description")]
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		[JsonProperty(PropertyName = "rate")]
		public string Rate
		{
			get { return _rate; }
			set { _rate = value; }
		}

		[JsonProperty(PropertyName = "movie_path")]
		public string MoviePath
		{
			get { return _moviePath; }
			set { _moviePath = value; }
		}

		[JsonProperty(PropertyName = "trailer_path")]
		public string TrailerPath
		{
			get { return _trailerPath; }
			set { _trailerPath = value; }
		}

		[JsonProperty(PropertyName = "release_date")]
		public string ReleaseDate
		{
			get { return _releaseDate; }
			set { _releaseDate = value; }
		}

		[JsonProperty(PropertyName = "banner")]
		public string Banner
		{
			get { return _banner; }
			set { _banner = value; }
		}

		[JsonProperty(PropertyName = "poster")]
		public string Poster
		{
			get { return _poster; }
			set { _poster = value; }
		}


	}
}

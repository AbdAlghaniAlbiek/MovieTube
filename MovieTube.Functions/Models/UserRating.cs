using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTube.Functions.Models
{
    public class UserRating
    {
        #region Fields
        private string _comment;
		private string _ratingPerformActors;
		private string _ratingResolution;
		private string _ratingEntertament;
		private string _liked;
        #endregion


        [JsonProperty(PropertyName = "liked")]
		public string Liked
		{
			get { return _liked; }
			set { _liked = value; }
		}

		[JsonProperty(PropertyName = "rating_entertament")]
		public string RatingEntertament
		{
			get { return _ratingEntertament; }
			set { _ratingEntertament = value; }
		}

		[JsonProperty(PropertyName = "rating_resolution")]
		public string RatingResolution
		{
			get { return _ratingResolution; }
			set { _ratingResolution = value; }
		}

		[JsonProperty(PropertyName = "rating_perform_actors")]
		public string RatingPerformActors
		{
			get { return _ratingPerformActors; }
			set { _ratingPerformActors = value; }
		}

		[JsonProperty(PropertyName = "comment")]
		public string Comment
		{
			get { return _comment; }
			set { _comment = value; }
		}
	}
}

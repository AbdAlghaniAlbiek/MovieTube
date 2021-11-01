using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTube.Functions.Models
{
    public class UsersFeedback
    {
        #region Fields
        private string _ratingPerformActors;
		private string _ratingResolution;
		private string _avgRatingEntertament;
		private string _countRatings;
		private string _countDislikes;
		private string _countLikes;
		private string _countComments;
		private int _id;
		#endregion

		[JsonProperty(PropertyName = "id")]
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		[JsonProperty(PropertyName = "count_comments")]
		public string CountComments
		{
			get { return _countComments; }
			set { _countComments = value; }
		}

		[JsonProperty(PropertyName = "count_like")]
		public string CountLikes
		{
			get { return _countLikes; }
			set { _countLikes = value; }
		}

		[JsonProperty(PropertyName = "count_dislike")]
		public string CountDislikes
		{
			get { return _countDislikes; }
			set { _countDislikes = value; }
		}

		[JsonProperty(PropertyName = "count_ratings")]
		public string CountRatings
		{
			get { return _countRatings; }
			set { _countRatings = value; }
		}

		[JsonProperty(PropertyName = "avg_rating_entertament")]
		public string AvgRatingEntertament
		{
			get { return _avgRatingEntertament; }
			set { _avgRatingEntertament = value; }
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

	}
}

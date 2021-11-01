using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTube.Functions.Models
{
    public class Movie
    {
		#region Fields
		private string _categoryName;
		private int _id;
		private string _name;
		private string _description;
		private string _rate;
		private string _imagePath;
		#endregion


		[JsonProperty(PropertyName = "id")]
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		[JsonProperty(PropertyName = "movie_name")]
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

		[JsonProperty(PropertyName = "image_path")]
		public string ImagePath
		{
			get { return _imagePath; }
			set { _imagePath = value; }
		}

		[JsonProperty(PropertyName = "category_name")]
		public string CategoryName
		{
			get { return _categoryName; }
			set { _categoryName = value; }
		}

		[JsonProperty(PropertyName = "rate")]
		public string Rate
		{
			get { return _rate; }
			set { _rate = value; }
		}
	}
}

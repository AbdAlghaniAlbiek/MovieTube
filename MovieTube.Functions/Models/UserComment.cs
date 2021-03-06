using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTube.Functions.Models
{
    public class UserComment
    {
        #region Fields
        private string _comment;
		private string _imagePath;
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

		[JsonProperty(PropertyName = "image_path")]
		public string ImagePath
		{
			get { return _imagePath; }
			set { _imagePath = value; }
		}

		[JsonProperty(PropertyName = "comment")]
		public string Comment
		{
			get { return _comment; }
			set { _comment = value; }
		}

	}
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTube.Functions.Models
{
    public class User
    {
        #region fields
        private string _dateTime;
		private string _name;
		private string _password;
		private string _email;
		private int _id;
		private string _token;
        #endregion

       
		[JsonProperty(PropertyName = "id")]
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		[JsonProperty(PropertyName = "email")]
		public string Email
		{
			get { return _email; }
			set { _email = value; }
		}

		[JsonProperty(PropertyName = "password")]
		public string Password
		{
			get { return _password; }
			set { _password = value; }
		}

		[JsonProperty(PropertyName = "name")]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		
		[JsonProperty(PropertyName = "dateJoin")]
		public string DateJoin
		{
			get { return _dateTime; }
			set { _dateTime = value; }
		}


		[JsonProperty(PropertyName = "token")]
		public string Token
		{
			get { return _token; }
			set { _token = value; }
		}

	}
}

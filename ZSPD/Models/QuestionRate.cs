using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZSPD.Models
{
	public class QuestionRate
	{
		public int Id { get; set; }

		public string Question { get; set; }

		public int? Grade { get; set; }
	}
}
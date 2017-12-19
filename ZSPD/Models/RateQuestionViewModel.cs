using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZSPD.Models
{
	public class RateQuestionViewModel
	{
		public List<QuestionRate> QuestionRates { get; set; }

		public string UserId { get; set; }
	}
}
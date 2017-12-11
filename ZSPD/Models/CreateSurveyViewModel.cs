
using ZSPD.Domain.Models.EntityModels;

namespace ZSPD.Models
{
    public class CreateSurveyViewModel
    {
        public Question Question { get; set; }
        public bool AddToSurvey { get; set; }
    }
}
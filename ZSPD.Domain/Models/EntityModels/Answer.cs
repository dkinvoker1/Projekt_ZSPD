﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZSPD.Domain.Models.EntityModels
{
    public class Answer
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        public int AnswerRate { get; set; }
    }
}

﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ZSPD.Domain.Models.EntityModels
{
    public class Grade
    {
        public int Id { get; set; }

        public virtual Question Question { get; set; }

        public int Value { get; set; }
	}
}

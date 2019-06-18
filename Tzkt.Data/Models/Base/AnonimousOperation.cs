﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Tzkt.Data.Models.Base
{
    public class AnonimousOperation : BaseOperation
    {
        public int BakerId { get; set; }

        #region relations
        [ForeignKey("BakerId")]
        public Contract Baker { get; set; }
        #endregion
    }
}
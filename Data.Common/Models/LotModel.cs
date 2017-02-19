namespace Data.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class LotModel : GenericModel<int>
    {
        public LotModel()
            : base()
        {
        }

        public LotModel(int lot)
            : this(lot.ToString())
        {
        }

        public LotModel(string lot)
            : this()
        {
            this.Lot = lot.ToString();
        }

        public string Lot { get; set; }
    }
}
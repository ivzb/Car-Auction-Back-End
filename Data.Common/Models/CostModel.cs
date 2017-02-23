namespace Data.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CostModel : GenericModel<int>
    {
        public CostModel()
            : base()
        {
        }

        public CostModel(int cost)
            : this()
        {
            this.Cost = cost;
        }

        public int Cost { get; set; }
    }
}
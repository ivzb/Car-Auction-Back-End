namespace Data.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ValueModel : GenericModel<int>
    {
        public ValueModel()
            : base()
        {
        }

        public ValueModel(string value)
            : this()
        {
            this.Value = value;
        }

        public string Value { get; set; }
    }
}
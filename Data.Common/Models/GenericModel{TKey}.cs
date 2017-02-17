namespace Data.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class GenericModel<TKey>
    {
        [Key]
        public TKey Id { get; set; }

        public string Value { get; set; }
    }
}
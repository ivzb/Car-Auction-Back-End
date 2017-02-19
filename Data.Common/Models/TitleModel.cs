namespace Data.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TitleModel : GenericModel<int>
    {
        public TitleModel()
            : base()
        {
        }

        public TitleModel(string title)
            : this()
        {
            this.Title = title;
        }

        public string Title { get; set; }
    }
}
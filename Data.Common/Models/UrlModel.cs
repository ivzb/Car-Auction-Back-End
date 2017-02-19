namespace Data.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UrlModel : GenericModel<int>
    {
        public UrlModel()
            : base()
        {
        }

        public UrlModel(string url)
            : this()
        {
            this.Url = url;
        }

        public string Url { get; set; }
    }
}
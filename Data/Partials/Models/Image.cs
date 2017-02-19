namespace Data
{
    using Data.Common.Models;
    using System;
    using System.Collections.Generic;

    public partial class Image : UrlModel
    {
        public Image()
            : base()
        {
        }

        public Image(string url)
            : base(url)
        {
        }
    }
}
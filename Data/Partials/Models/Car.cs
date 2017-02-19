namespace Data
{
    using Data.Common.Models;
    using System;
    using System.Collections.Generic;

    public partial class Car : LotModel
    {
        public Car(string lot)
            : base(lot)
        {
        }
    }
}
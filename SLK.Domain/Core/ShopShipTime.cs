﻿using System;

namespace SLK.Domain.Core
{
    public class ShopShipTime
    {
        protected ShopShipTime() { }

        public int ID { get; protected set; }

        public int ShopID { get; protected set; }

        public virtual Shop Shop { get; protected set; }

        public int Day { get; protected set; }

        public DateTime TimeFrom { get; protected set; }

        public DateTime TimeTo { get; protected set; }

        public bool IsSpecial { get; protected set; }

        public DateTime Date { get; protected set; }

        public bool Active { get; protected set; }
    }
}

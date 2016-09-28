using System;
using System.Collections.Generic;

namespace SLK.Domain.Core
{
    public class Order
    {
        protected Order() { }

        public Order(User user, Shop shop)
        {
            User = user;
            Shop = shop;

            CreatedOn = DateTime.Now;
        }

        public long ID { get; protected set; }

        public long UserID { get; protected set; }

        public virtual User User { get; protected set; }

        public long ShopID { get; protected set; }

        public virtual Shop Shop { get; protected set; }

        public bool ShipOn { get; protected set; }

        public DateTime ShipTime { get; protected set; }

        public string ShipAddress { get; protected set; }

        public string FullName { get; protected set; }

        public string Phone { get; protected set; }

        public string Email { get; protected set; }

        public DateTime CreatedOn { get; protected set; }

        public DateTime PayedOn { get; protected set; }

        public DateTime SentOn { get; protected set; }

        public DateTime DeliveredOn { get; protected set; }

        public int PaymentMethod { get; protected set; }

        public int ShipmentMethod { get; protected set; }

        public decimal Total { get; protected set; }

        public decimal SubTotal { get; protected set; }

        public decimal ShipCost { get; protected set; }

        public string CouponCode { get; protected set; }

        public decimal TotalDiscountAmount { get; protected set; }

        public string TotalDiscountDescription { get; set; }

        public decimal ShipDiscountAmount { get; protected set; }

        public string ShipDiscountDescription { get; set; }

        public int Status { get; protected set; }

        public string Comment { get; protected set; }

        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}

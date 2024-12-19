namespace POS_SYSTEM
{
    internal class OrderItem
    {
        internal decimal ItemPrice;

        public string ItemName { get; internal set; }
        public decimal Total { get; internal set; }
        public int Quantity { get; internal set; }
    }
}
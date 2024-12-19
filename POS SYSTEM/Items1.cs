namespace POS_SYSTEM
{
    internal class Items
    {
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public int ItemID { get; set; }
        public string ItemCategory { get; set; }
        public int ItemStock { get; set; }
        public byte[] ItemImage { get; internal set; }
    }
}
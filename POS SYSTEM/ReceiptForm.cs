using System;

namespace POS_SYSTEM
{
    internal class ReceiptForm
    {
        public object ReceiptItems { get; set; }
        public decimal GrandTotal { get; set; }

        internal void ShowDialog()
        {
            throw new NotImplementedException();
        }
    }
}
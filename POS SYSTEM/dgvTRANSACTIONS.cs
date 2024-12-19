using System.Collections.Generic;
using System.Windows.Forms;

namespace POS_SYSTEM
{
    internal class dgvTRANSACTIONS
    {
        public static IEnumerable<DataGridViewRow> Rows { get; internal set; }
        public static object Columns { get; internal set; }
    }
}
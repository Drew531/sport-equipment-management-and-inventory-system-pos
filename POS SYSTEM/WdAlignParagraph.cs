using Microsoft.Office.Interop.Word;

namespace POS_SYSTEM
{
    internal class WdAlignParagraph
    {
        internal static WdParagraphAlignment wdAlignParagraphCenter;

        public static WdParagraphAlignment Center { get; internal set; }
        public static WdParagraphAlignment Right { get; internal set; }
    }
}
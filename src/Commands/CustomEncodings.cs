using System.Text;

namespace DocumentMargin.Commands
{
    internal partial class EncodingMenuCommand
    {
        public class UTF8WithBomEncoding : UTF8Encoding
        {
            public UTF8WithBomEncoding() : base(true)
            { }

            public override string EncodingName => "Unicode (UTF-8 with signature)";

            public override string BodyName => "UTF-8 with BOM";
        }

        public class UTF8WithoutBomEncoding : UTF8Encoding
        {
            public UTF8WithoutBomEncoding() : base(false)
            { }

            public override string EncodingName => "Unicode (UTF-8 without signature)";
        }
    }
}
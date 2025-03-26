using System.Buffers;
using System.Text;
using SuperSocket.ProtoBase;

namespace LabTestBrowser.UI;

public class MllpPipelineFilter() : BeginEndMarkPipelineFilter<TextPackageInfo>(BeginMark, EndMark)
{
	private static readonly byte[] BeginMark = [0x0B];
	private static readonly byte[] EndMark = [0x1C, 0x0D];

	protected override TextPackageInfo DecodePackage(ref ReadOnlySequence<byte> buffer)
	{
		var message = Encoding.UTF8.GetString(buffer);

		return new TextPackageInfo
		{
			Text = message
		};
	}
}
using System.Buffers;
using SuperSocket.ProtoBase;

namespace LabTestBrowser.Desktop.Mllp;

public class MllpPipelineFilter() : BeginEndMarkPipelineFilter<MllpPackage>(BeginMark, EndMark)
{
	private static readonly byte[] BeginMark = [0x0B];
	private static readonly byte[] EndMark = [0x1C, 0x0D];

	protected override MllpPackage DecodePackage(ref ReadOnlySequence<byte> buffer)
	{
		return new MllpPackage
		{
			Content = buffer.ToArray()
		};
	}
}
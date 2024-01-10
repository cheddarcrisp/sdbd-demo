namespace SDBD;

public class Codec : ICodec {
  public Document Decode(byte[] data) {
    using var input = new MemoryStream(data);

    var version = input.ReadByte();

    return version switch {
      0x01 => DecodeV1(input),
      _ => throw new Exception("Unsupported version")
    };
  }

  public byte[] Encode(Document document) {
    var dataLength = document.Data.Length;
    var contentLength = new Dictionary<string, string> () {
      { "content-length", dataLength.ToString() }
    };
    var headers = document.Metadata.Union(contentLength);

    var packedHeaders = packHeaders(headers);
    var headerLength = Convert.ToUInt16(packedHeaders.Length);

    using var output = new MemoryStream();
    output.WriteByte(0x01);
    output.Write(BitConverter.GetBytes(headerLength));
    output.Write(packedHeaders);
    output.Write(document.Data);

    return output.ToArray();
  }

  private Document DecodeV1(Stream stream) {
    var headerLengthBytes = new byte[2];
    stream.ReadExactly(headerLengthBytes);

    var headerLength = BitConverter.ToUInt16(headerLengthBytes);

    var headerBytes = new byte[headerLength];
    stream.ReadExactly(headerBytes);

    var headers = unpackHeaders(headerBytes);
    string contentLengthString;
    headers.Remove("content-length", out contentLengthString);
    var contentLength = int.Parse(contentLengthString);

    var data = new byte[contentLength];
    stream.ReadExactly(data);

    return new Document(headers, data);
  }

  private Dictionary<string, string> unpackHeaders(byte[] packedHeaders) {
    var decoder = new hpack.Decoder(8192, 4096);
    var listener = new HeaderListener();

    using var reader = new BinaryReader(new MemoryStream(packedHeaders));
    decoder.Decode(reader, listener);
    decoder.EndHeaderBlock();

    return listener.Headers;
  }

  private byte[] packHeaders(IEnumerable<KeyValuePair<string, string>> headers) {
    var encoder = new hpack.Encoder(0); //0 will disable dynamic table that we don't need anyways

    using var output = new MemoryStream();
    using var writer = new BinaryWriter(output);

    foreach(var (name, value) in headers) {
      encoder.EncodeHeader(writer, name, value);
    }

    return output.ToArray();
  }
}

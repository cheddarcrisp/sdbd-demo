namespace SDBD;

public class Codec : ICodec {
  public Document Decode(byte[] data) {
    return new(
      new () {
        { "content-name", "text.txt" }
      },
      data
    );
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

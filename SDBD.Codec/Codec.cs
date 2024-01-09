namespace SDBD;

public class Codec : ICodec {
  public Document Decode(byte[] data) {
    return new(new (), new byte[0]);
  }

  public byte[] Encode(Document document) {
    return new byte[0];
  }
}

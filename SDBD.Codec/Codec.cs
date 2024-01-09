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
    return document.Data;
  }
}

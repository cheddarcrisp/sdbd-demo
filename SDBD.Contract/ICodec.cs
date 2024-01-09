namespace SDBD;

public interface ICodec {
  Document Decode(byte[] data);
  byte[] Encode(Document document);
}
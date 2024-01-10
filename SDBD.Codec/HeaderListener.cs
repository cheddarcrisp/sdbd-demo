using System.Text;

internal class HeaderListener : hpack.IHeaderListener {
  public Dictionary<string, string> Headers { get; private set; }

  public HeaderListener() {
    Headers = new Dictionary<string, string>();
  }

  public void AddHeader(byte[] nameBytes, byte[] valueBytes, bool sensitive) {
    var name = Encoding.UTF8.GetString(nameBytes);
    var value = Encoding.UTF8.GetString(valueBytes);

    Headers.Add(name, value);
  }
}

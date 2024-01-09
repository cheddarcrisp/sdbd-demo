SDBD.ICodec codec = new SDBD.Codec();

var (encode, filename) = ParseArgs(args);
var inputData = File.ReadAllBytes(filename);

if(encode) {
  SDBD.Document document = new (
    new() {
      { "content-name", filename }
    },
    inputData
  );
  var outputData = codec.Encode(document);
  File.WriteAllBytes($"{filename}.sbdb", outputData);
} else {
  var document = codec.Decode(inputData);
  File.WriteAllBytes(document.Metadata["content-name"], document.Data);
}

(bool encode, string filename) ParseArgs(string[] args) {
  return args switch {
    [var filename] => (true, filename),
    ["-d", var filename] => (false, filename),
    ["-e", var filename] => (true, filename),
    _ => throw new Exception("I don't like those arguments")
  };
}
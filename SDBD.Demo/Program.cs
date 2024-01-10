SDBD.ICodec codec = new SDBD.Codec();

var (encode, filepath) = ParseArgs(args);
var inputData = File.ReadAllBytes(filepath);
var filename = Path.GetFileName(filepath);

if(encode) {
  SDBD.Document document = new (
    new() {
      { "content-name", filename }
    },
    inputData
  );
  var outputData = codec.Encode(document);
  File.WriteAllBytes($"{filename}.sdbd", outputData);
} else {
  var document = codec.Decode(inputData);
  File.WriteAllBytes(document.Metadata["content-name"], document.Data);
}

(bool encode, string filepath) ParseArgs(string[] args) {
  return args switch {
    [var filepath] => (true, filepath),
    ["-d", var filepath] => (false, filepath),
    ["-e", var filepath] => (true, filepath),
    _ => throw new Exception("I don't like those arguments")
  };
}

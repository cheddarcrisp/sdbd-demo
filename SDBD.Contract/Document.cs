namespace SDBD;

public record Document(
    Dictionary<string, string> Metadata,
    byte[] Data
);

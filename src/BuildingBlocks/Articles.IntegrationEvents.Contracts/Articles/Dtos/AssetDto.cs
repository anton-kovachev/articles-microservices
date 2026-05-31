using Articles.Abstractions.Enums;

namespace Articles.IntregationEvents.Contracts.Articles.Dtos;

public record AssetDto(int Id, string Name, AssetType Type, FileDto File);

public record FileDto(string OriginalName, string Name, string Extension, string FileServerId, long Size);

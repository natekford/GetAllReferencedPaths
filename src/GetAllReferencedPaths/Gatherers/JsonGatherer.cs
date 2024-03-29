﻿using System.Text.Json;

namespace GetAllReferencedPaths.Gatherers;

public sealed class JsonGatherer : GathererBase
{
	public JsonGatherer(IEnumerable<DirectoryInfo> roots) : base(roots)
	{
	}

	public override async Task<Result<List<string>>> GetStringsAsync(
		FileInfo source,
		CancellationToken cancellationToken = default)
	{
		try
		{
			using var fs = source.OpenRead();
			using var doc = await JsonDocument.ParseAsync(
				utf8Json: fs,
				cancellationToken: cancellationToken
			);

			return new(GetStrings(doc.RootElement).ToList());
		}
		catch
		{
			return new("Unable to parse JSON.");
		}
	}

	private static IEnumerable<string> GetStrings(JsonElement element)
	{
		return element.ValueKind switch
		{
			JsonValueKind.Object => element.EnumerateObject().SelectMany(x => GetStrings(x.Value)),
			JsonValueKind.Array => element.EnumerateArray().SelectMany(x => GetStrings(x)),
			JsonValueKind.String => new[] { element.GetString()! },
			_ => Array.Empty<string>(),
		};
	}
}
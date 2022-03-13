﻿using CSharpNotion.Api.Response;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CSharpNotion
{
    internal static class Utils
    {
        public static string ExtractId(string id)
        {
            if (!id.Contains('-'))
                return id[..8] + "-" + id[8..12] + "-" + id[12..16] + "-" + id[16..20] + "-" + id[20..];
            return id;
        }

        public static void SetHttpContent(ref HttpRequestMessage httpRequest, object data)
        {
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(data, Client.SerializeOptions));
            httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        public static Entities.BaseBlock ConvertBlockFromResponse(Client client, RecordMapBlockValue blockValue)
        {
            if (blockValue.Type is null) throw new ArgumentNullException("blockValue.Type can't be null");
            Type blockType = Constants.TypeToBlock.GetValueOrDefault(blockValue.Type, typeof(Entities.DividerBlock));
            return (Entities.BaseBlock)Activator.CreateInstance(blockType, client, blockValue)!;
        }

        public static string RecieveTitle(RecordMapBlockValue blockValue)
        {
            return blockValue?.Properties?.Title?.ElementAt(0)[0].GetString() ?? "";
        }
    }
}
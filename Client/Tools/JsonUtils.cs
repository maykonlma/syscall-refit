using System.Text;

namespace Client.Tools;

public static class JsonUtils
{
    public static string ExtractJsonFromResponse(string responseContent)
    {
        var parts = responseContent.Split(["\r\n\r\n"], 2, StringSplitOptions.None);
        if (parts.Length < 2)
        {
            throw new InvalidOperationException("Invalid HTTP response, body not found.");
        }
        
        return DecodeChunkedBody(parts[1]);
    }

    private static string DecodeChunkedBody(string chunkedBody)
    {
        var decodedBody = new StringBuilder();
        var reader = new StringReader(chunkedBody);

        while (true)
        {
            string? chunkSizeLine = reader.ReadLine();
            if (string.IsNullOrEmpty(chunkSizeLine))
            {
                break;
            }

            var chunkSize = int.Parse(chunkSizeLine, System.Globalization.NumberStyles.HexNumber);
            if (chunkSize == 0)
            {
                break;
            }

            var chunkBuffer = new char[chunkSize];
            var read = reader.Read(chunkBuffer, 0, chunkSize);
            decodedBody.Append(chunkBuffer, 0, read);
            
            reader.ReadLine();
        }

        return decodedBody.ToString();
    }
}
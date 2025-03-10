using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace Auctions.Features.ImageModeration.SignEngine;

public class SignEngine : IImageModeration
{
    public static readonly string HttpClientName = "SignEngineClient";
    private readonly IHttpClientFactory _clientFactory;
    private readonly SignEngineOptions _signEngineOptions;

    public SignEngine(IHttpClientFactory clientFactory, IOptions<SignEngineOptions> options)
    {
        _clientFactory = clientFactory;
        _signEngineOptions = options.Value;
    }

    public async Task<bool> CheckImage(MemoryStream image, string objectName, string contentType,
        CancellationToken ct = default)
    {
        var client = _clientFactory.CreateClient(HttpClientName);
        
        using var content = new MultipartFormDataContent
        {
            { new StringContent(_signEngineOptions.Models), "models" },
            { new StringContent(_signEngineOptions.ApiUser), "api_user" },
            { new StringContent(_signEngineOptions.ApiSecret), "api_secret" }
        };


        var streamContent = new StreamContent(image);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        content.Add(streamContent, "media", objectName); 

        var response = await client.PostAsync("/1.0/check.json", content, ct);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(response.ReasonPhrase);
            return false;
        }

        var result = await response.Content.ReadFromJsonAsync<RootObject>(ct);
        return result != null && result.status == "success";
    }
}

public class SignEngineResponse
{
    [JsonPropertyName("status")] public required string Status { get; init; }
}

public class RootObject
{
    public string status { get; set; }
    public Request request { get; set; }
    public Recreational_drug recreational_drug { get; set; }
    public Medical medical { get; set; }
    public Offensive offensive { get; set; }
    public Faces[] faces { get; set; }
    public Gore gore { get; set; }
    public Text text { get; set; }
    public Violence violence { get; set; }
    public Self_harm self_harm { get; set; }
    public Media media { get; set; }
}

public class Request
{
    public string id { get; set; }
    public double timestamp { get; set; }
    public int operations { get; set; }
}

public class Recreational_drug
{
    public double prob { get; set; }
    public Classes classes { get; set; }
}

public class Classes
{
    public double cannabis { get; set; }
    public double cannabis_logo_only { get; set; }
    public double cannabis_plant { get; set; }
    public double cannabis_drug { get; set; }
    public double recreational_drugs_not_cannabis { get; set; }
}

public class Medical
{
    public double prob { get; set; }
    public Classes1 classes { get; set; }
}

public class Classes1
{
    public double pills { get; set; }
    public double paraphernalia { get; set; }
}

public class Offensive
{
    public double nazi { get; set; }
    public double asian_swastika { get; set; }
    public double confederate { get; set; }
    public double supremacist { get; set; }
    public double terrorist { get; set; }
    public double middle_finger { get; set; }
}

public class Faces
{
    public double x1 { get; set; }
    public double y1 { get; set; }
    public double x2 { get; set; }
    public double y2 { get; set; }
    public Features features { get; set; }
    public Attributes attributes { get; set; }
}

public class Features
{
    public Left_eye left_eye { get; set; }
    public Right_eye right_eye { get; set; }
    public Nose_tip nose_tip { get; set; }
    public Left_mouth_corner left_mouth_corner { get; set; }
    public Right_mouth_corner right_mouth_corner { get; set; }
}

public class Left_eye
{
    public double x { get; set; }
    public double y { get; set; }
}

public class Right_eye
{
    public double x { get; set; }
    public double y { get; set; }
}

public class Nose_tip
{
    public double x { get; set; }
    public double y { get; set; }
}

public class Left_mouth_corner
{
    public double x { get; set; }
    public double y { get; set; }
}

public class Right_mouth_corner
{
    public double x { get; set; }
    public double y { get; set; }
}

public class Attributes
{
    public double minor { get; set; }
    public double sunglasses { get; set; }
    public double male { get; set; }
    public double female { get; set; }
}

public class Gore
{
    public double prob { get; set; }
    public Classes2 classes { get; set; }
    public Type type { get; set; }
}

public class Classes2
{
    public double very_bloody { get; set; }
    public double slightly_bloody { get; set; }
    public double body_organ { get; set; }
    public double serious_injury { get; set; }
    public double superficial_injury { get; set; }
    public double corpse { get; set; }
    public double skull { get; set; }
    public double unconscious { get; set; }
    public double body_waste { get; set; }
    public double other { get; set; }
}

public class Type
{
    public double animated { get; set; }
    public double fake { get; set; }
    public double real { get; set; }
}

public class Text
{
    public object[] profanity { get; set; }
    public object[] personal { get; set; }
    public object[] link { get; set; }
    public object[] social { get; set; }
    public object[] extremism { get; set; }
    public object[] medical { get; set; }
    public object[] drug { get; set; }
    public object[] weapon { get; set; }
    public object[] content_trade { get; set; }
    public object[] money_transaction { get; set; }
    public object[] spam { get; set; }
    public object[] violence { get; set; }
    public object[] self_harm { get; set; }
    public bool ignored_text { get; set; }
}

public class Violence
{
    public double prob { get; set; }
    public Classes3 classes { get; set; }
}

public class Classes3
{
    public double physical_violence { get; set; }
    public double firearm_threat { get; set; }
    public double combat_sport { get; set; }
}

public class Self_harm
{
    public double prob { get; set; }
    public Type1 type { get; set; }
}

public class Type1
{
    public double real { get; set; }
    public double fake { get; set; }
    public double animated { get; set; }
}

public class Media
{
    public string id { get; set; }
    public string uri { get; set; }
}


// See https://aka.ms/new-console-template for more information

using System;
using System.Text.Json;

Console.WriteLine("WRITE AN USER NAME");

var username = Console.ReadLine();

HttpClient client = new();
client.DefaultRequestHeaders.Add("User-Agent", "C# App");
var response = await client.GetAsync($"https://api.github.com/users/{username}/events");
response.EnsureSuccessStatusCode();
string data = await response.Content.ReadAsStringAsync();
var events = JsonDocument.Parse(data).RootElement;

foreach (var item in events.EnumerateArray())
{
    string action = string.Empty;
    string type = item.GetProperty("type").GetString();
    string repo = item.GetProperty("repo").GetProperty("name").GetString();

    Console.WriteLine($"TYPE -> {type}");
    switch (type)
    {
        case "PushEvent":
            int commitcount = item.GetProperty("payload").GetProperty("commits").GetArrayLength();
            string author = item.GetProperty("actor").GetProperty("display_login").GetString();
            action = $"Pushed {commitcount} to {repo}: Author: {author}";
            break;

        case "CreateEvent":
            action = $"Created {item.GetProperty("payload").GetProperty("ref_type").GetString()} in {repo}";
            break;

        case "Watchitem":
            action = $"Starred {repo}";
            break;

        default:
            action = $"{type.Replace("item", "")} in {repo}";
            break;

    }
    Console.WriteLine(action);
}

Console.WriteLine(events);
// JuanCorreaRodriguez 
Console.ReadLine();

//{
//  "id":"50306806654",
//	"type":"PushEvent",
//	"actor":{
//      "id":89568825,
//		"login":"JuanCorreaRodriguez",
//		"display_login":"JuanCorreaRodriguez",
//		"gravatar_id":"",
//		"url":"https://api.github.com/users/JuanCorreaRodriguez",
//		"avatar_url":"https://avatars.githubusercontent.com/u/89568825?"

//    },
//	"repo":	{
//      "id":989812222,
//		"name":"JuanCorreaRodriguez/GBSLibrary",
//		"url":"https://api.github.com/repos/JuanCorreaRodriguez/GBSLibrary"

//    },
//	"payload":{
//      "repository_id":989812222,
//		"push_id":24595298687,
//		"size":1,
//		"distinct_size":1,
//		"ref":"refs/heads/main",
//		"head":"20211fd06c5a7a0e9f21f372e92ab05f9918e86f",
//		"before":"6dc8fe45b72a9ede8ebb1774af6519996538ac8e",
//		"commits":[{
//          "sha":"20211fd06c5a7a0e9f21f372e92ab05f9918e86f",
//			"author":{
//              "email":"codeli.studio@gmail.com",
//				"name":"CodeliStudio"
//          },
//			"message":"1.1",
//			"distinct":true,
//			"url":"https://api.github.com/repos/JuanCorreaRodriguez/GBSLibrary/commits/20211fd06c5a7a0e9f21f372e92ab05f9918e86f"
//        }]
//	},
//	"public":true,
//	"created_at":"2025-05-29T23:56:41Z"
//}
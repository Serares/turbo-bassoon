HttpClient client = new();
HttpResponseMessage response = await client.GetAsync("https://www.apple.com");
WriteLine($"Apple's home page has {response.Content.Headers.ContentLength} bytes.");

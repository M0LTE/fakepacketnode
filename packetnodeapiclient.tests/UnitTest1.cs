using FluentAssertions;
using packetnodeapiclient.xrouter;
using System.Diagnostics;

namespace packetnodeapiclient.tests
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            XrouterClient client = new("http://debian-i386:9080", new HttpClient());

            ICollection<MessageListing> messageList = await client.GetMessageListAsync();

            foreach (MessageListing listing in messageList)
            {
                try
                {
                    Message message = await client.GetMessageAsync(listing.Id);
                    Debug.WriteLine(message.Text![..50] + "...");
                }
                catch (ApiException ex) when (ex.Message.StartsWith("Could not deserialize the response body stream"))
                {
                    // deserialisation issue, skip for now.
                }
            }

            PostResponse response = await client.PostMessageAsync(new PostMessage
            {
                From = "M0LTE",
                To = "G8PZT",
                Subject = "Hello world",
                Type = "P",
                Text = "This is a message"
            });

            Debug.WriteLine($"id:{response.Id}");
        }
    }
}
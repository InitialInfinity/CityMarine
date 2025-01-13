using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CityMarineService.Models
{
    public class GraphApiEmailResponse
    {
        [JsonProperty("value")]
        public List<GraphApiMessage> Value { get; set; }

        public class GraphApiMessage
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("subject")]
            public string Subject { get; set; }

            [JsonProperty("from")]
            public GraphApiEmailAddressWrapper From { get; set; }

            [JsonProperty("toRecipients")]
            public List<GraphApiEmailRecipient> ToRecipients { get; set; }

            [JsonProperty("body")]
            public GraphApiBody Body { get; set; }

            [JsonProperty("receivedDateTime")]
            public DateTime? ReceivedDateTime { get; set; }

            [JsonProperty("isRead")]
            public bool IsRead { get; set; }

            [JsonProperty("inReplyToId")]
            public string InReplyToId { get; set; }

            [JsonProperty("attachments")]
            public List<GraphApiAttachment> Attachments { get; set; }
        }

        // Adjusted to match JSON structure for the "from" property.
        public class GraphApiEmailAddressWrapper
        {
            [JsonProperty("emailAddress")]
            public GraphApiEmailAddress EmailAddress { get; set; }
        }

        public class GraphApiEmailAddress
        {
            [JsonProperty("address")]
            public string Address { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class GraphApiEmailRecipient
        {
            [JsonProperty("emailAddress")]
            public GraphApiEmailAddress EmailAddress { get; set; }
        }

        public class GraphApiBody
        {
            [JsonProperty("contentType")]
            public string ContentType { get; set; }

            [JsonProperty("content")]
            public string Content { get; set; }
        }

        public class GraphApiAttachment
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("contentType")]
            public string ContentType { get; set; }

            [JsonProperty("size")]
            public long Size { get; set; }

            // Add property for the attachment content (e.g., URL or contentBytes)
            [JsonProperty("contentBytes")]
            public string ContentBytes { get; set; }  // Base64 encoded content (if applicable)

            // Alternatively, you might have a download URL
            [JsonProperty("contentUrl")]
            public string ContentUrl { get; set; }
        }

        public class GraphApiFolderResponse
        {
            [JsonProperty("value")]
            public List<GraphApiFolder> Value { get; set; }
        }

        public class GraphApiFolder
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }
        }
    }
}

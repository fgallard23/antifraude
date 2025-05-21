using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Domain.Messages
{
    public abstract class Message
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } // Id by Transacction
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Boyner.CaseStudy.ApplicationCore.Entities
{
    public class Post : BaseEntity
    {
        public Post()
        {
            
        }
        public Post(string id, string text)
        {
            Id = id;
            Text = text;
        }

        public Post(string text)
        {
            Text = text;
        }

        public string Text { get; set; }

    }

    public class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }


        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }



    }
}
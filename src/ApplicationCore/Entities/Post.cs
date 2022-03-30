using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Boyner.CaseStudy.ApplicationCore.Entities
{
    public class Post : BaseEntity
    {
        
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
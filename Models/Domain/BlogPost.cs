namespace SecureKnowledgeMAnagementSystemv1.API.Models.Domain{
    
    public class BlogPost
    {
        public Guid id {get; set; }
        public string tittle {get; set; }
        public string ShortDescription {get; set; }

        public string content {get; set; }

        public string FeaturedInageUrl {get; set; }

        public string UrlHandle {get; set; }

        public DateTime PublishedDate {get; set; }

        public string Author {get; set; }
        public bool IsVisible {get; set; }

    }
}
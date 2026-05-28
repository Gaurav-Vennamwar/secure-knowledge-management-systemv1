export interface AddBlogPostRequest{
    tittle: string;
    shortDescription : string;
    content : string;
    featuredImageUrl: string;
    urlHandle: string;
    author: string;
    publishedDate: string;
    isVisible: boolean;
}
export interface BlogPost{
    id : string;
    Tittle: string;
    ShortDescription : string;
    Content : string;
    FeaturedImageUrl: string;
    UrlHandle: string;
    Author: string;
    PublishedDate: string;
    IsVisible: boolean;
}
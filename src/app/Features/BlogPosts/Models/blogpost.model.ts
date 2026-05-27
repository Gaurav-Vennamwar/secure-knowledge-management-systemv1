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
    tittle: string;
    shortDescription : string;
    content : string;
    featuredImageUrl: string;
    urlHandle: string;
    author: string;
    publishedDate: string;
    isVisible: boolean;
}
import { Category } from '../../Category/Models/category.models';

export interface AddBlogPostRequest {
  Tittle: string;
  ShortDescription: string;
  Content: string;
  FeaturedImageUrl: string;
  UrlHandle: string;
  Author: string;
  PublishedDate: string;
  IsVisible: boolean;
  Categories: string[];
}
export interface UpdateBlogPostRequest {
  Tittle: string;
  ShortDescription: string;
  Content: string;
  FeaturedImageUrl: string;
  UrlHandle: string;
  Author: string;
  PublishedDate: string;
  IsVisible: boolean;
  Categories: string[];
}
export interface BlogPost {
  Id: string;
  Tittle: string;
  ShortDescription: string;
  Content: string;
  FeaturedImageUrl: string;
  UrlHandle: string;
  Author: string;
  PublishedDate: string;
  IsVisible: boolean;
  Categories: Category[];
}
export interface PaginatedBlogPostResponse {
  Items: BlogPost[];
  TotalCount: number;
  PageNumber: number;
  PageSize: number;
  TotalPages: number;
}



export interface AddCategoryRequest {
  Name: string;
  UrlHandle: string;
}

export interface UpdateCategoryRequest {
  Name: string;
  UrlHandle: string;
}
export interface Category {
  id: string;
  Name: string;
  UrlHandle: string;
}

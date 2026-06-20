  import { HttpClient, httpResource, HttpResourceRef } from '@angular/common/http';
  import { inject, Injectable, InputSignal, Signal } from '@angular/core';
  import { environment } from '../../../../environments/environment';
  import { AddBlogPostRequest, ApiResponse, BlogPost, PaginatedBlogPostResponse, UpdateBlogPostRequest } from '../Models/blogpost.model';
  import { Observable } from 'rxjs';
import { StringTokenKind } from '@angular/compiler';

  @Injectable({
    providedIn: 'root',
  })
  export class BlogPostService {
    http = inject(HttpClient);
    apiBaseUrl = environment.apiBaseUrl;

    //service method to craete a blog post
    createBlogPost(data : AddBlogPostRequest) : Observable<BlogPost>{
      return this.http.post<BlogPost>(`${this.apiBaseUrl}/api/blogpost`, data,{
        withCredentials : true
      });
    }

    //service method to get all blog posts
   getAllBlogPosts(pageNumber: Signal<number>, pageSize: number = 10): HttpResourceRef<ApiResponse<PaginatedBlogPostResponse> | undefined> {
  return httpResource<ApiResponse<PaginatedBlogPostResponse>>(() =>
    `${this.apiBaseUrl}/api/blogpost?pageNumber=${pageNumber()}&pageSize=${pageSize}`
  );
}

    //service method to get a single id of blog post
    getBlogPostById(id : InputSignal<string | undefined>) :HttpResourceRef<BlogPost | undefined>{
     return httpResource<BlogPost>(() => `${this.apiBaseUrl}/api/blogpost/${id()}` );
    }

    //service method to get all blog posts by url handle
    getBlogPostByUrlHandle(urlHandle : InputSignal<string | undefined>) :HttpResourceRef<BlogPost | undefined>{
      return httpResource<BlogPost>(() => `${this.apiBaseUrl}/api/blogpost/${urlHandle()}` );
    }

    //service method to edit the blog post
    editBlogPost(id : string, body : UpdateBlogPostRequest) : Observable<BlogPost>{
      return this.http.put<BlogPost>(`${this.apiBaseUrl}/api/blogpost/${id}` , body,{
        withCredentials : true
      });
    }

    //servicce method to delete blog post
    deleteBlogpost(id : string) : Observable<BlogPost>{
      return this.http.delete<BlogPost>(`${this.apiBaseUrl}/api/blogpost/${id}`,
        {
          withCredentials : true
        }
      );
    }
  }

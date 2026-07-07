import { HttpClient, httpResource, HttpResourceRef } from '@angular/common/http';
import { inject, Injectable, InputSignal, Signal } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { AddBlogPostRequest, BlogPost, PaginatedBlogPostResponse, UpdateBlogPostRequest } from '../Models/blogpost.model';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../Models/api-reponse-models';

@Injectable({
  providedIn: 'root',
})
export class BlogPostService {
  http = inject(HttpClient);
  apiBaseUrl = environment.apiBaseUrl;

  //service method to create a blog post
  createBlogPost(data: AddBlogPostRequest): Observable<ApiResponse<BlogPost>> {
    return this.http.post<ApiResponse<BlogPost>>(`${this.apiBaseUrl}/api/blogpost`, data, {
      withCredentials: true
    });
  }

  //service method to get all blog posts
  getAllBlogPosts(pageNumber: Signal<number>, pageSize: number = 10): HttpResourceRef<ApiResponse<PaginatedBlogPostResponse> | undefined> {
    return httpResource<ApiResponse<PaginatedBlogPostResponse>>(() =>
      `${this.apiBaseUrl}/api/blogpost?pageNumber=${pageNumber()}&pageSize=${pageSize}`
    );
  }

  //service method to get a single blog post by id
  getBlogPostById(id: InputSignal<string | undefined>): HttpResourceRef<ApiResponse<BlogPost> | undefined> {
    return httpResource<ApiResponse<BlogPost>>(() => `${this.apiBaseUrl}/api/blogpost/${id()}`);
  }

  //service method to get a blog post by url handle
  getBlogPostByUrlHandle(urlHandle: InputSignal<string | undefined>): HttpResourceRef<ApiResponse<BlogPost> | undefined> {
    return httpResource<ApiResponse<BlogPost>>(() => `${this.apiBaseUrl}/api/blogpost/${urlHandle()}`);
  }

  //service method to edit the blog post
  editBlogPost(id: string, body: UpdateBlogPostRequest): Observable<ApiResponse<BlogPost>> {
    return this.http.put<ApiResponse<BlogPost>>(`${this.apiBaseUrl}/api/blogpost/${id}`, body, {
      withCredentials: true
    });
  }

  //service method to delete blog post
  deleteBlogpost(id: string): Observable<ApiResponse<BlogPost>> {
    return this.http.delete<ApiResponse<BlogPost>>(`${this.apiBaseUrl}/api/blogpost/${id}`, {
      withCredentials: true
    });
  }
  getBlogPostByUrlHandleHttp(urlHandle: string): Observable<ApiResponse<BlogPost>> {
  return this.http.get<ApiResponse<BlogPost>>(
    `${this.apiBaseUrl}/api/blogpost/${urlHandle}`
  );
}
}
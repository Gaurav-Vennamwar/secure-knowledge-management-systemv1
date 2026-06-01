  import { HttpClient, httpResource, HttpResourceRef } from '@angular/common/http';
  import { inject, Injectable, InputSignal } from '@angular/core';
  import { environment } from '../../../../environments/environment';
  import { AddBlogPostRequest, BlogPost } from '../Models/blogpost.model';
  import { Observable } from 'rxjs';

  @Injectable({
    providedIn: 'root',
  })
  export class BlogPostService {
    http = inject(HttpClient);
    apiBaseUrl = environment.apiBaseUrl;

    //service method to craete a blog post
    createBlogPost(data : AddBlogPostRequest) : Observable<BlogPost>{
      return this.http.post<BlogPost>(`${this.apiBaseUrl}/api/blogpost`, data);
    }

    //service method to get all blog posts
    getAllBlogPosts(): HttpResourceRef<BlogPost[] | undefined>{
      return httpResource<BlogPost[]>(() => `${this.apiBaseUrl}/api/blogpost`);
    }

    //service method to get a single id of blog post
    getBlogPostById(id : InputSignal<string | undefined>) :HttpResourceRef<BlogPost | undefined>{
     return httpResource<BlogPost>(() => `${this.apiBaseUrl}/api/blogpost/${id()}` );
    }
  }

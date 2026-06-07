import { Component, inject, input, resource } from '@angular/core';
import { BlogPostService } from '../../BlogPosts/Services/blog-post-service';
import { DatePipe } from '@angular/common';
import { MarkdownComponent } from "ngx-markdown";

@Component({
  selector: 'app-blog-details',
  imports: [DatePipe, MarkdownComponent],
  templateUrl: './blog-details.html',
  styleUrl: './blog-details.css',
})
export class BlogDetails {
  url = input<string | undefined>();

  blogpPostService = inject(BlogPostService);
  //fetch the details using url
  blogDetailRef = this.blogpPostService.getBlogPostByUrlHandle(this.url);
  isLoading = this.blogDetailRef.isLoading;
  blogDetailResponse = this.blogDetailRef.value;

   
}

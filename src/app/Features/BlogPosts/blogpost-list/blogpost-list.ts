import { Component, inject, signal } from '@angular/core';
import { RouterLink } from "@angular/router";
import { BlogPostService } from '../Services/blog-post-service';

@Component({
  selector: 'app-blogpost-list',
  imports: [RouterLink],
  templateUrl: './blogpost-list.html',
  styleUrl: './blogpost-list.css',
})
export class BlogpostList {
  blogPostService = inject(BlogPostService);

  PageNumber = signal(1);
  PageSize = 10;

  getAllBlogPostRef = this.blogPostService.getAllBlogPosts(this.PageNumber,this.PageSize);

  isLoading = this.getAllBlogPostRef.isLoading;
  error = this.getAllBlogPostRef.error;
  response = this.getAllBlogPostRef.value;
  statusCode =this.getAllBlogPostRef.statusCode;

  nextPage() {
    const totalPages = this.response()?.Data?.TotalPages ?? 1;
    if (this.PageNumber() < totalPages) {
      this.PageNumber.update(p => p + 1);
    }
  }

  previousPage() {
    if (this.PageNumber() > 1) {
      this.PageNumber.update(p => p - 1);
    }
  }
}

import { Component, inject, signal } from '@angular/core';
import { BlogPostService } from '../../BlogPosts/Services/blog-post-service';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-home',
  imports: [RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  blogPostService = inject(BlogPostService);

  pageNumber = signal(1);
  pageSize = 10;

  blogPostRef = this.blogPostService.getAllBlogPosts(this.pageNumber, this.pageSize);
  isLoading = this.blogPostRef.isLoading;
  blogPostResponse = this.blogPostRef.value;

  nextPage() {
    const totalPages = this.blogPostResponse()?.Data?.TotalPages ?? 1;
    if (this.pageNumber() < totalPages) {
      this.pageNumber.update(p => p + 1);
    }
  }

  previousPage() {
    if (this.pageNumber() > 1) {
      this.pageNumber.update(p => p - 1);
    }
  }

}

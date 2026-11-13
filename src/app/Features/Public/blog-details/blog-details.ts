import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { MarkdownComponent } from 'ngx-markdown';

import { BlogPostService } from '../../BlogPosts/Services/blog-post-service';
import { BlogPost } from '../../BlogPosts/Models/blogpost.model';

@Component({
  selector: 'app-blog-details',
  imports: [DatePipe, MarkdownComponent, RouterLink],
  templateUrl: './blog-details.html',
  styleUrl: './blog-details.css',
})
export class BlogDetails implements OnInit {
  private route = inject(ActivatedRoute);
  private blogPostService = inject(BlogPostService);
  private cdr = inject(ChangeDetectorRef); // ← add this

  blogPost?: BlogPost;
  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const url = params.get('url');
      if (!url) {
        this.isLoading = false;
        return;
      }

      this.blogPostService.getBlogPostByUrlHandleHttp(url).subscribe({
        next: (response) => {
          this.blogPost = response.Data;
          this.isLoading = false;
          this.cdr.detectChanges(); // ← force UI update
        },
        error: (err) => {
          this.errorMessage = err.status === 404
            ? 'This article is not available. Its link may be outdated or the post was never published.'
            : 'We could not load this article right now. Please try again in a moment.';
          this.isLoading = false;
          this.cdr.detectChanges();
        }
      });
    });
  }
}

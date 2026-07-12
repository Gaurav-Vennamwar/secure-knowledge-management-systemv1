import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';
import { MarkdownComponent } from 'ngx-markdown';

import { BlogPostService } from '../../BlogPosts/Services/blog-post-service';
import { BlogPost } from '../../BlogPosts/Models/blogpost.model';

@Component({
  selector: 'app-blog-details',
  imports: [DatePipe, MarkdownComponent],
  templateUrl: './blog-details.html',
  styleUrl: './blog-details.css',
})
export class BlogDetails implements OnInit {
  private route = inject(ActivatedRoute);
  private blogPostService = inject(BlogPostService);
  private cdr = inject(ChangeDetectorRef); // ← add this

  blogPost?: BlogPost;
  isLoading = true;

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const url = params.get('url');
      if (!url) {
        this.isLoading = false;
        return;
      }

      this.blogPostService.getBlogPostByUrlHandleHttp(url).subscribe({
        next: (response) => {
          console.log('RESPONSE:', response);
          this.blogPost = response.Data;
          this.isLoading = false;
          this.cdr.detectChanges(); // ← force UI update
        },
        error: (err) => {
          console.error('ERROR:', err);
          this.isLoading = false;
          this.cdr.detectChanges();
        }
      });
    });
  }
}

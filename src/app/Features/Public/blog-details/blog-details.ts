import { Component, inject, OnInit } from '@angular/core';
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

  blogPost?: BlogPost;

  isLoading = true;

  ngOnInit(): void {

    const url = this.route.snapshot.paramMap.get('url');

    if (!url) return;

    this.blogPostService.getBlogPostByUrlHandleHttp(url).subscribe({
      next: (response) => {
        this.blogPost = response.Data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });

  }

}
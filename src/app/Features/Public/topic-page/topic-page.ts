import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { BlogPostService } from '../../BlogPosts/Services/blog-post-service';

@Component({ selector: 'app-topic-page', imports: [RouterLink], templateUrl: './topic-page.html', styleUrl: './topic-page.css' })
export class TopicPage {
  private route = inject(ActivatedRoute);
  private blogPostService = inject(BlogPostService);
  topic = signal(''); pageNumber = signal(1); pageSize = 9;
  topicResource = this.blogPostService.getBlogPostsByCategory(this.topic, this.pageNumber, this.pageSize);
  isLoading = this.topicResource.isLoading; posts = this.topicResource.value;
  constructor() { this.route.paramMap.subscribe(params => { this.topic.set(params.get('topic') ?? ''); this.pageNumber.set(1); }); }
  get title() { return this.topic().replace(/-/g, ' ').replace(/\b\w/g, letter => letter.toUpperCase()); }
  nextPage(totalPages: number) { if (this.pageNumber() < totalPages) this.pageNumber.update(page => page + 1); }
  previousPage() { if (this.pageNumber() > 1) this.pageNumber.update(page => page - 1); }
}

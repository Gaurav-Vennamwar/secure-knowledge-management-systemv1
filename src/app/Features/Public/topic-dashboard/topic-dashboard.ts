import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CategoryService } from '../../Category/Services/category-service';

@Component({ selector: 'app-topic-dashboard', imports: [RouterLink], templateUrl: './topic-dashboard.html', styleUrl: './topic-dashboard.css' })
export class TopicDashboard {
  private categoryService = inject(CategoryService);
  categoryResource = this.categoryService.getAllCategories();
  isLoading = this.categoryResource.isLoading;
  categories = this.categoryResource.value;
}

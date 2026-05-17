import { HttpClient, httpResource } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { AddCategoryRequest, Category } from '../Models/category.models';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private http = inject(HttpClient);
  private apiBaseUrl = "http://localhost:5251";

  addCategoryStatus = signal<'idle' | 'loading' | 'error' | 'success'>('idle');

  addCategory(category : AddCategoryRequest){
    this.addCategoryStatus.set('loading');
     this.http.post<void>(`${this.apiBaseUrl}/api/Categories`, category ).subscribe({
      next : () => {
        this.addCategoryStatus.set('success');
      },
      error : (err) => {
        console.log(err);
        this.addCategoryStatus.set('error');
      }
     });
     
  }

 getAllCategories() {
  const x = httpResource<Category[]>(
    () => `${this.apiBaseUrl}/api/Categories`
  );

  return x;
}
}

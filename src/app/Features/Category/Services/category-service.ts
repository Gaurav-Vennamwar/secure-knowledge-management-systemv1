import { HttpClient, httpResource } from '@angular/common/http';
import { inject, Injectable, InputSignal, signal } from '@angular/core';
import { AddCategoryRequest, Category, UpdateCategoryRequest } from '../Models/category.models';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../Models/api-reponse-models';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private http = inject(HttpClient);
  private apiBaseUrl = environment.apiBaseUrl;

  addCategoryStatus = signal<'idle' | 'loading' | 'error' | 'success'>('idle');
  updateCategoryStatus = signal<'idle' | 'loading' | 'error' | 'success'>('idle');

  addCategory(category: AddCategoryRequest) {
    this.addCategoryStatus.set('loading');
    this.http.post<ApiResponse<Category>>(`${this.apiBaseUrl}/api/Categories`, category, {
      withCredentials: true
    }).subscribe({
      next: () => {
        this.addCategoryStatus.set('success');
      },
      error: (err) => {
        console.log(err);
        this.addCategoryStatus.set('error');
      },
    });
  }

  getAllCategories() {
    return httpResource<ApiResponse<Category[]>>(() => `${this.apiBaseUrl}/api/Categories`);
  }

  getCategoryById(id: InputSignal<string | undefined>) {
    return httpResource<ApiResponse<Category>>(() => `${this.apiBaseUrl}/api/Categories/${id()}`);
  }

  updateCategory(id: String, updateCategoryRequestDto: UpdateCategoryRequest) {
    this.updateCategoryStatus.set('loading')
    this.http.put<ApiResponse<Category>>(`${this.apiBaseUrl}/api/Categories/${id}`, updateCategoryRequestDto, {
      withCredentials: true
    })
    .subscribe({
      next: () => {
        this.updateCategoryStatus.set('success');
      },
      error: () => {
        this.updateCategoryStatus.set('error');
      },
    });
  }

  //delete category service
  deleteCategory(id: string): Observable<ApiResponse<Category>> {
    return this.http.delete<ApiResponse<Category>>(`${this.apiBaseUrl}/api/Categories/${id}`, {
      withCredentials: true
    });
  }
}
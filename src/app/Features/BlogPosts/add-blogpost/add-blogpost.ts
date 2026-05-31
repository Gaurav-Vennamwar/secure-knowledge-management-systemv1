import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { BlogPostService } from '../Services/blog-post-service';
import { AddBlogPostRequest } from '../Models/blogpost.model';
import { Router } from '@angular/router';

import { MarkdownComponent } from 'ngx-markdown';

import { CategoryService } from '../../Category/Services/category-service';

import { NgSelectModule } from '@ng-select/ng-select';

@Component({
  selector: 'app-add-blogpost',

  standalone: true,

  imports: [ReactiveFormsModule, MarkdownComponent, NgSelectModule],

  templateUrl: './add-blogpost.html',

  styleUrl: './add-blogpost.css',
})
export class AddBlogpost {
  blogPostService = inject(BlogPostService);

  categoryService = inject(CategoryService);

  router = inject(Router);

  private categoriesResourceRef = this.categoryService.getAllCategories();

  categoriesResponse = this.categoriesResourceRef.value;

  addBlogPostForm = new FormGroup({
    tittle: new FormControl<string>('', {
      nonNullable: true,

      validators: [Validators.required, Validators.minLength(7), Validators.maxLength(100)],
    }),

    shortDescription: new FormControl<string>('', {
      nonNullable: true,

      validators: [Validators.required, Validators.minLength(10), Validators.maxLength(200)],
    }),

    content: new FormControl<string>('', {
      nonNullable: true,

      validators: [Validators.required, Validators.minLength(10)],
    }),

    featuredImageUrl: new FormControl<string>('', {
      nonNullable: true,

      validators: [Validators.required, Validators.maxLength(200)],
    }),

    urlHandle: new FormControl<string>('', {
      nonNullable: true,

      validators: [Validators.required, Validators.maxLength(200)],
    }),

    publishedDate: new FormControl<string>(new Date().toISOString().split('T')[0], {
      nonNullable: true,

      validators: [Validators.required],
    }),

    author: new FormControl<string>('', {
      nonNullable: true,

      validators: [Validators.required, Validators.maxLength(100)],
    }),

    isVisible: new FormControl<boolean>(true, {
      nonNullable: true,
    }),

    // 🔥 NG SELECT CATEGORY IDS
    categories: new FormControl<string[]>([], {
      nonNullable: true,
    }),
  });

  onSubmit() {
    console.log('SUBMIT CLICKED');

    if (!this.addBlogPostForm.valid) {
      console.log(this.addBlogPostForm);

      return;
    }

    const formRawValue = this.addBlogPostForm.getRawValue();

    console.log('Selected Categories:', formRawValue.categories);

    const requestDto: AddBlogPostRequest = {
      tittle: formRawValue.tittle,

      shortDescription: formRawValue.shortDescription,

      content: formRawValue.content,

      author: formRawValue.author,

      featuredImageUrl: formRawValue.featuredImageUrl,

      isVisible: formRawValue.isVisible,

      urlHandle: formRawValue.urlHandle,

      publishedDate: formRawValue.publishedDate,

      // we will send this when backend DTO supports it
      // categories: formRawValue.categories
    };

    console.log(JSON.stringify(requestDto));

    this.blogPostService.createBlogPost(requestDto).subscribe({
      next: (response) => {
        console.log('SUCCESS');

        console.log(response);

        this.router.navigate(['/admin/blogposts']);
      },

      error: (err) => {
        console.log(err);

        console.log(err.error);
      },
    });
  }
}

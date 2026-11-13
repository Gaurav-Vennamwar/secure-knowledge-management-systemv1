import { Component, effect, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { BlogPostService } from '../Services/blog-post-service';
import { AddBlogPostRequest } from '../Models/blogpost.model';
import { Router } from '@angular/router';
import { MarkdownComponent } from 'ngx-markdown';
import { CategoryService } from '../../Category/Services/category-service';
import { NgSelectModule } from '@ng-select/ng-select';
import { ImageSelectorService } from '../../../Shared/Services/image-selector-service';

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

  imageSlectorService = inject(ImageSelectorService);

  isPublishing = signal(false);
  submitError = signal<string | null>(null);

   //whenever the signal changes effecct gets called and it will repatch the new value
//whenever the image gets selectedimages signal changes then the effect fetches the url 
selectedImageEffectRef = effect(() => {
  const selectedImageUrl = this.imageSlectorService.selectedImage();//selected image ismeh store kiya
  if(selectedImageUrl){//if its valid then 
    this.addBlogPostForm.patchValue({
      FeaturedImageUrl : selectedImageUrl
    });
  }

})

  private categoriesResourceRef = this.categoryService.getAllCategories();

  categoriesResponse = this.categoriesResourceRef.value;

  addBlogPostForm = new FormGroup({
    Tittle: new FormControl<string>('', { nonNullable: true }),

    ShortDescription: new FormControl<string>('', { nonNullable: true }),

    Content: new FormControl<string>('', { nonNullable: true }),

    FeaturedImageUrl: new FormControl<string>('', { nonNullable: true }),

    UrlHandle: new FormControl<string>('', { nonNullable: true }),

    PublishedDate: new FormControl<string>(new Date().toISOString().split('T')[0], { nonNullable: true }),

    Author: new FormControl<string>('', { nonNullable: true }),

    IsVisible: new FormControl<boolean>(true, {
      nonNullable: true,
    }),

    // 🔥 NG SELECT CATEGORY IDS
    Categories: new FormControl<string[]>([], {
      nonNullable: true,
    }),
  });

  onSubmit() {
    this.submitError.set(null);

    const formRawValue = this.addBlogPostForm.getRawValue();
    const requestDto: AddBlogPostRequest = {
      Tittle: formRawValue.Tittle,

      ShortDescription: formRawValue.ShortDescription,

      Content: formRawValue.Content,

      Author: formRawValue.Author,

      FeaturedImageUrl: formRawValue.FeaturedImageUrl,

      IsVisible: formRawValue.IsVisible,

      UrlHandle: formRawValue.UrlHandle,

      PublishedDate: formRawValue.PublishedDate,

      // we will send this when backend DTO supports it
      Categories: formRawValue.Categories ?? [],
    };

    this.isPublishing.set(true);
    this.blogPostService.createBlogPost(requestDto).subscribe({
      next: (response) => {
        this.router.navigate(['/admin/blogposts']);
      },

      error: (err) => {
        this.isPublishing.set(false);
        if (err.status === 401 || err.status === 403) {
          this.submitError.set('Your writer session has expired or does not have permission to publish. Please sign in again with your writer account.');
          return;
        }

        this.submitError.set(
          err.error?.Message || err.error?.message || 'We could not publish this post right now. Please check the details and try again.'
        );
      },
    });
  }
}

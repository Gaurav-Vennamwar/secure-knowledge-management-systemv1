import { Component, effect, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
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
    Tittle: new FormControl<string>('', {
      nonNullable: true,

      validators: [Validators.required, Validators.minLength(7), Validators.maxLength(100)],
    }),

    ShortDescription: new FormControl<string>('', {
      nonNullable: true,

      validators: [Validators.required, Validators.minLength(10), Validators.maxLength(200)],
    }),

    Content: new FormControl<string>('', {
      nonNullable: true,

      validators: [Validators.required, Validators.minLength(10)],
    }),

    FeaturedImageUrl: new FormControl<string>('', {
      nonNullable: true,

      validators: [Validators.required, Validators.maxLength(200)],
    }),

    UrlHandle: new FormControl<string>('', {
      nonNullable: true,

      validators: [Validators.required, Validators.maxLength(200)],
    }),

    PublishedDate: new FormControl<string>(new Date().toISOString().split('T')[0], {
      nonNullable: true,

      validators: [Validators.required],
    }),

    Author: new FormControl<string>('', {
      nonNullable: true,

      validators: [Validators.required, Validators.maxLength(100)],
    }),

    IsVisible: new FormControl<boolean>(true, {
      nonNullable: true,
    }),

    // 🔥 NG SELECT CATEGORY IDS
    Categories: new FormControl<string[]>([], {
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

    console.log('Selected Categories:', formRawValue.Categories);

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

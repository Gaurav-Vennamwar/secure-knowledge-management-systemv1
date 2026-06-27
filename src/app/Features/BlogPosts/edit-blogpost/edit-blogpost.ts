import { ChangeDetectorRef, Component, effect, inject, input } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { BlogPostService } from '../Services/blog-post-service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MarkdownComponent } from 'ngx-markdown';
import { NgSelectModule } from '@ng-select/ng-select';
import { CategoryService } from '../../Category/Services/category-service';
import { UpdateBlogPostRequest } from '../Models/blogpost.model';
import { ImageSelectorService } from '../../../Shared/Services/image-selector-service';

@Component({
  selector: 'app-edit-blogpost',
  imports: [ReactiveFormsModule, MarkdownComponent, NgSelectModule],
  templateUrl: './edit-blogpost.html',
  styleUrl: './edit-blogpost.css',
})
export class EditBlogpost {
  id = input<string>();
  blogPostService = inject(BlogPostService);
  categoryService = inject(CategoryService);
  imageSelectorService = inject(ImageSelectorService);
  router = inject(Router);
   cdr = inject(ChangeDetectorRef);

  private blogPostRef = this.blogPostService.getBlogPostById(this.id);
  blogPostResponse = this.blogPostRef.value;

  private categoriesRef = this.categoryService.getAllCategories();
  categoriesResponse = this.categoriesRef.value;

  editBlogPostForm = new FormGroup({
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
    Categories: new FormControl<string[]>([], {
      nonNullable: true,
    }),
  });
  effectRef = effect(() => {
    const post = this.blogPostResponse();
    if (post) {
      this.editBlogPostForm.patchValue({
        Tittle: post.Data?.Tittle,
        ShortDescription: post.Data?.ShortDescription,
        Content: post.Data?.Content,
        Author: post.Data?.Author,
        PublishedDate: post.Data?.PublishedDate.substring(0, 10),
        IsVisible: post.Data?.IsVisible,
        UrlHandle: post.Data?.UrlHandle,
        FeaturedImageUrl: post.Data?.FeaturedImageUrl,
        Categories: post.Data?.Categories.map((x) => x.Id),
      });
      this.cdr.detectChanges();
    }
  });
  //whenever the signal changes effecct gets called and it will repatch the new value
  //whenever the image gets selectedimages signal changes then the effect fetches the url
  selectedImageEffectRef = effect(() => {
    const selectedImageUrl = this.imageSelectorService.selectedImage(); //selected image ismeh store kiya
    if (selectedImageUrl) {
      //if its valid then
      this.editBlogPostForm.patchValue({
        FeaturedImageUrl: selectedImageUrl,
      });
    }
  });

  onSubmit() {
    const id = this.id();
    if (id && this.editBlogPostForm.valid) {
      const formValue = this.editBlogPostForm.getRawValue();

      const updateBlogPostRequestDto: UpdateBlogPostRequest = {
        Tittle: formValue.Tittle,
        Content: formValue.Content,
        ShortDescription: formValue.ShortDescription,
        Author: formValue.Author,
        FeaturedImageUrl: formValue.FeaturedImageUrl,
        IsVisible: formValue.IsVisible,
        PublishedDate: formValue.PublishedDate,
        UrlHandle: formValue.UrlHandle,
        Categories: formValue.Categories ?? [],
      };
      this.blogPostService.editBlogPost(id, updateBlogPostRequestDto).subscribe({
        next: (response) => {
          this.router.navigate(['/admin/blogposts']);
        },
        error: () => {
          console.error('Something Went Wromg!');
        },
      });
    }
  }
  onDelete() {
    const id = this.id();
    if (id) {
      this.blogPostService.deleteBlogpost(id).subscribe({
        next: (response) => {
          console.log(response);
          this.router.navigate(['/admin/blogposts']);
        },
        error() {
          console.error('Something went wrong ');
        },
      });
    }
  }
  openImageSelector() {
    this.imageSelectorService.displayImageSelector();
  }
}

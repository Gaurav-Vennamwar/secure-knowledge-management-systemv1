import { Component, effect, inject, input } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { BlogPostService } from '../Services/blog-post-service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MarkdownComponent } from 'ngx-markdown';
import { NgSelectModule } from '@ng-select/ng-select';
import { CategoryService } from '../../Category/Services/category-service';
import { UpdateBlogPostRequest } from '../Models/blogpost.model';
import { ImageSelector } from '../../../Shared/Components/image-selector/image-selector';
import { ImageSelectorService } from '../../../Shared/Services/image-selector-service';

@Component({
  selector: 'app-edit-blogpost',
  imports: [ReactiveFormsModule, MarkdownComponent, NgSelectModule,ImageSelector],
  templateUrl: './edit-blogpost.html',
  styleUrl: './edit-blogpost.css',

})
export class EditBlogpost {
  id = input<string>();
  blogPostService = inject(BlogPostService);
  categoryService = inject(CategoryService);
  imageSelectorService = inject(ImageSelectorService)
  router = inject(Router);


  private blogPostRef = this.blogPostService.getBlogPostById(this.id);
  blogPostResponse = this.blogPostRef.value;

  private categoriesRef = this.categoryService.getAllCategories();
  categoriesResponse = this.categoriesRef.value;

  editBlogPostForm = new FormGroup({
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

  effectRef = effect(() =>{
    const post = this.blogPostResponse();
    if(post){
    this.editBlogPostForm.patchValue({
      tittle : this.blogPostResponse()?.Tittle,
      shortDescription : this.blogPostResponse()?.ShortDescription,
      content : this.blogPostResponse()?.Content,
      author : this.blogPostResponse()?.Author,
      publishedDate : this.blogPostResponse()?.PublishedDate,
      isVisible : this.blogPostResponse()?.IsVisible,
      urlHandle : this.blogPostResponse()?.UrlHandle,
      featuredImageUrl : this.blogPostResponse()?.FeaturedImageUrl,
      categories : this.blogPostResponse()?.Categories.map(x => x.Id),
      
    });
  }
  });

  onSubmit(){
    const id = this.id();
if(id && this.editBlogPostForm.valid){

   const formValue = this.editBlogPostForm.getRawValue();

   const updateBlogPostRequestDto : UpdateBlogPostRequest ={
    tittle :formValue.tittle,
    content :formValue.content,
    shortDescription : formValue.shortDescription,
    author : formValue.author,
    featuredImageUrl : formValue.featuredImageUrl,
    isVisible : formValue.isVisible,
    publishedDate : formValue.publishedDate,
    urlHandle : formValue.urlHandle,
    categories : formValue.categories ?? []
  };
  this.blogPostService.editBlogPost(id , updateBlogPostRequestDto).
  subscribe({
    next : (response) => {
      this.router.navigate(['/admin/blogposts']);
    },
    error : ()=>{
      console.error('Something Went Wromg!');
    }
  })
}
  }
  onDelete(){
    const id = this.id();
    if(id){
      this.blogPostService.deleteBlogpost(id).
      subscribe({
        next : (response) =>
        {
         console.log(response);
         this.router.navigate(['/admin/blogposts']); 
        },error(){
          console.error("Something went wrong ");
        }
      })
    }
  }
openImageSelector(){
    this.imageSelectorService.displayImageSelector();
}

}

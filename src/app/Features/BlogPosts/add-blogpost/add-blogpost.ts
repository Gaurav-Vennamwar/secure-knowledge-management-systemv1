import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BlogPostService } from '../Services/blog-post-service';
import { AddBlogPostRequest } from '../Models/blogpost.model';
import { Router } from '@angular/router';
import { MarkdownComponent } from 'ngx-markdown';

@Component({
  selector: 'app-add-blogpost',
  imports: [ReactiveFormsModule, MarkdownComponent],
  templateUrl: './add-blogpost.html',
  styleUrl: './add-blogpost.css',
})
export class AddBlogpost {
  blogPostService = inject(BlogPostService);
  router = inject(Router);

  addBlogPostForm = new FormGroup({
    tittle: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(7), Validators.maxLength(100)],
    }),
    shortDescription: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(10), Validators.maxLength(200)],
    }),
    content: new FormControl<string>(' ', {
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
  });
  onSubmit(){
    console.log("SUBMIT CLICKED");

    if (!this.addBlogPostForm.valid) {
    console.log(this.addBlogPostForm);
    return;
  }
    const formRawValue = this.addBlogPostForm.getRawValue();

    const requestDto : AddBlogPostRequest = {
      tittle : formRawValue.tittle,
      shortDescription : formRawValue.shortDescription,
      content : formRawValue.content,
      author : formRawValue.author,
      featuredImageUrl : String(formRawValue.featuredImageUrl),
      isVisible : formRawValue.isVisible,
      urlHandle : formRawValue.urlHandle,
     publishedDate : formRawValue.publishedDate

    }
console.log(JSON.stringify(requestDto));
    this.blogPostService.createBlogPost(requestDto).
    subscribe({
      next : (response) => {
        console.log("SUCCESS");
        console.log(response);
          console.log(response);

          //navigate back to blog post list page
          this.router.navigate(['/admin/blogposts']);
      },
      error : (err) => {
        console.log(err);
  console.log(err.error);
      }
    });
    
  }
}

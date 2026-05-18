import { Component, effect, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Validators } from '@angular/forms';
import { AddCategoryRequest } from '../Models/category.models';
import { CategoryService } from '../Services/category-service';
import { DefaultUrlSerializer, Router } from '@angular/router';

@Component({
  selector: 'app-add-category',
  imports: [ReactiveFormsModule],
  templateUrl: './add-category.html',
  styleUrl: './add-category.css',
})
export class AddCategory {

  private router = inject(Router);

  constructor(){
    effect(() => {
      if (this.categoryService.addCategoryStatus() === 'success') {
          this.categoryService.addCategoryStatus.set('idle')
          this.router.navigate(['/admin/categories']);//when su then renavigate    
      }
      if (this.categoryService.addCategoryStatus() === 'error') {
        console.error("Adding category request failed");
      }
    });

  }
  
  private categoryService = inject(CategoryService)

  addcategoryFormGroup = new FormGroup({
    name : new FormControl<string>('', { nonNullable : true, validators: [Validators.required, Validators.maxLength(50)]}),
    urlHandle : new FormControl<string>('', { 
      nonNullable : true,
      validators: [Validators.required, Validators.maxLength(100)]})
  });

  get nameFormContrtol(){
    return this.addcategoryFormGroup.controls.name;
  }

  get urlHandleFormContrtol(){
    return this.addcategoryFormGroup.controls.urlHandle;
  }
  onSubmit(){
    const addCategoryFormValue = this.addcategoryFormGroup.getRawValue();

    const addCategoryRequestDto : AddCategoryRequest = {
      name: addCategoryFormValue.name,
      urlHandle : addCategoryFormValue.urlHandle
    };

    this.categoryService.addCategory(addCategoryRequestDto);

    
  }


  

}

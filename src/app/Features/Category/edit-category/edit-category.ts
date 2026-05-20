import { Component, effect, inject, input } from '@angular/core';
import { CategoryService } from '../Services/category-service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UpdateCategoryRequest } from '../Models/category.models';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-edit-category',
  imports: [ReactiveFormsModule],
  templateUrl: './edit-category.html',
  styleUrl: './edit-category.css',
})
export class EditCategory {
  constructor() {
    effect(() => {
      if (this.categoryService.updateCategoryStatus() === 'success') {
        this.categoryService.updateCategoryStatus.set('idle');
        this.router.navigate(['admin/categories']);

      }
      if (this.categoryService.updateCategoryStatus() === 'error') {
         this.categoryService.updateCategoryStatus.set('idle');
        console.error('something went wrong');
      }
    });
  }

  id = input<string>();
  private categoryService = inject(CategoryService);
  private router = inject(Router);

  categoryResourceRef = this.categoryService.getCategoryById(this.id);
  categoryResponse = this.categoryResourceRef.value;

  editCategoryFormGroup = new FormGroup({
    name: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.maxLength(50)],
    }),
    urlHandle: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.maxLength(100)],
    }),
  });

  get nameFormContrtol() {
    return this.editCategoryFormGroup.controls.name;
  }

  get urlHandleFormContrtol() {
    return this.editCategoryFormGroup.controls.urlHandle;
  }

  //to fetch the changes effects are helpfull cuz they react to the changes which we have
  effectRef = effect(() => {
    this.editCategoryFormGroup.controls.name.patchValue(this.categoryResponse()?.name ?? '');
    this.editCategoryFormGroup.controls.urlHandle.patchValue(
      this.categoryResponse()?.urlHandle ?? '',
    );
  });

  onSubmit() {
    // Get actual string value from signal
    const id = this.id();

    // Stop if form invalid or id missing
    if (!this.editCategoryFormGroup.valid || !id) {
      return;
    }

    // Get form values
    const formRawValue = this.editCategoryFormGroup.getRawValue();

    // Create DTO
    const updateCategoryRequestDto: UpdateCategoryRequest = {
      name: formRawValue.name,
      urlHandle: formRawValue.urlHandle,
    };

    // Call service
    this.categoryService.updateCategory(id, updateCategoryRequestDto);
  }
}

import { Component, inject } from '@angular/core';
import { ImageSelectorService } from '../../Services/image-selector-service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { form } from '@angular/forms/signals';

@Component({
  selector: 'app-image-selector',
  imports: [ReactiveFormsModule],
  templateUrl: './image-selector.html',
  styleUrl: './image-selector.css',
})
export class ImageSelector {
  private imageSelectorService = inject(ImageSelectorService);
  showImageSelector = this.imageSelectorService.showImageSelector.asReadonly();

  imageSelectormUploadForm = new FormGroup({
    file: new FormControl<File | null | undefined>(null, {
      nonNullable: true,
      validators: [Validators.required],
    }),
    name: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.maxLength(100)],
    }),
    tittle: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required, Validators.maxLength(100)],
    }),
  });

  closeImageSelector() {
    this.imageSelectorService.closeImageSelector();
  }
  onFileSelected(event : Event){
    const input = event.target as HTMLInputElement;
    if(!input.files || input.files.length == 0){
      return;
    }
    const file = input.files[0];
    this.imageSelectormUploadForm.patchValue({
      file : file
    });
  }
  onSubmit() {
    if (this.imageSelectormUploadForm.valid) {
      //submit this form
      const formRawValue = this.imageSelectormUploadForm.getRawValue();
      this.imageSelectorService.uploadImage(
        formRawValue.file!, // ! to tell file is not null or not undefined
        formRawValue.name,
        formRawValue.tittle,
      ).subscribe({
        next : (reponse) => {
          console.log(reponse)
        },
        error :() =>{
          console.error("Something Went Wrong")
        }
      });
    }
  }
}

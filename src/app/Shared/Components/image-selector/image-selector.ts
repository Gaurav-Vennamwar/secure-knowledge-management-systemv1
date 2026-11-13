import { Component, inject, signal } from '@angular/core';
import { ImageSelectorService } from '../../Services/image-selector-service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BlogImage } from '../../Model/image.model';

@Component({
  selector: 'app-image-selector',
  imports: [ReactiveFormsModule],
  templateUrl: './image-selector.html',
  styleUrl: './image-selector.css',
})
export class ImageSelector {
  private imageSelectorService = inject(ImageSelectorService);
  showImageSelector = this.imageSelectorService.showImageSelector.asReadonly();
  
  
  //refreshing the preview page
  id = signal<string | undefined>(undefined);
  //calling get all images
  imageRef = this.imageSelectorService.getAllImage(this.id);
  isLoading = this.imageRef.isLoading;
  images = this.imageRef.value;
  isUploading = signal(false);
  uploadError = signal<string | null>(null);
  selectedFileName = signal('No file selected');
  previewUrl = signal<string | null>(null);

  

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
    const fileName = file.name.replace(/\.[^/.]+$/, '');
    this.imageSelectormUploadForm.patchValue({
      file : file,
      name: this.imageSelectormUploadForm.controls.name.value || fileName,
      tittle: this.imageSelectormUploadForm.controls.tittle.value || fileName,
    });
    this.selectedFileName.set(file.name);
    this.previewUrl.set(URL.createObjectURL(file));
  }

  onSelectImage(image : BlogImage){
    this.imageSelectorService.selectImage(image.Url);
  }
  onSubmit() {
    this.uploadError.set(null);
    if (this.imageSelectormUploadForm.invalid) {
      this.imageSelectormUploadForm.markAllAsTouched();
      this.uploadError.set('Choose an image, then add a file name and title.');
      return;
    }

    if (this.imageSelectormUploadForm.valid) {
      //submit this form
      const formRawValue = this.imageSelectormUploadForm.getRawValue();
      this.isUploading.set(true);
      this.imageSelectorService.uploadImage(
        formRawValue.file!, // ! to tell file is not null or not undefined
        formRawValue.name,
        formRawValue.tittle,
      ).subscribe({
        next : (reponse) => {
          this.id.set(reponse.Id);//unique string is coming back then the httpresource will always refresh
          this.imageSelectormUploadForm.reset();
          this.isUploading.set(false);
          this.previewUrl.set(null);
          this.selectedFileName.set('No file selected');
          this.imageSelectorService.selectImage(reponse.Url);
        },
        error :(error) =>{
          this.isUploading.set(false);
          this.uploadError.set(
            error?.error?.errors?.file?.[0] ||
            error?.error?.file?.[0] ||
            error?.error?.detail ||
            error?.error?.Message ||
            error?.error?.message ||
            'Upload failed. Check the API service logs, then try again.'
          );
        }
      });
    }
  }

}

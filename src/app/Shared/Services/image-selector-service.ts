import { HttpClient, httpResource, HttpResourceRef } from '@angular/common/http';
import { inject, Injectable, signal, WritableSignal } from '@angular/core';
import { Observable } from 'rxjs';
import { BlogImage } from '../Model/image.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ImageSelectorService {
  http = inject(HttpClient)
  showImageSelector = signal<boolean>(false);
  selectedImage = signal<string | null>(null);

  displayImageSelector(){
    this.showImageSelector.set(true);
  }
  closeImageSelector(){
    this.showImageSelector.set(false);
  }
  uploadImage(file : File, fileName : string, tittle : string) : Observable<BlogImage>{
    //this will basically submit all this data to api
    const formData = new FormData();
    formData.append('file', file);
    formData.append('fileName', fileName);
    formData.append('tittle', tittle);

  
    return this.http.post<BlogImage>(`${environment.apiBaseUrl}/api/images`, formData)
  }
  
  getAllImage(id : WritableSignal<string | undefined>) : HttpResourceRef<BlogImage[] | undefined>{
    return httpResource<BlogImage[]>(() =>{
      id();
      return `${environment.apiBaseUrl}/api/images `;
    })
  }

  //setting the signal and also hidding the image selector
  selectImage(imageUrl : string){
    this.selectedImage.set(imageUrl);
    this.closeImageSelector();
  }
}

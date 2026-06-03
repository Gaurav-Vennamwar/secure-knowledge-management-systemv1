import { Component, inject } from '@angular/core';
import { ImageSelectorService } from '../../Services/image-selector-service';

@Component({
  selector: 'app-image-selector',
  imports: [],
  templateUrl: './image-selector.html',
  styleUrl: './image-selector.css',
})
export class ImageSelector {
  private imageSelectorService = inject(ImageSelectorService);
  showImageSelector = this.imageSelectorService.showImageSelector.asReadonly();

  closeImageSelector(){
    this.imageSelectorService.closeImageSelector();
  }
}

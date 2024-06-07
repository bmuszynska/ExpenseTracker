import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-new-category',
  templateUrl: './new-category.component.html',
  styleUrls: ['./new-category.component.scss']
})
export class NewCategoryComponent {

  categoryPopUp = false;
  categoryName = '';
  @Output() categoryAdded = new EventEmitter<string>();
  @Output() closed = new EventEmitter<void>();

  openPopUp() {
    this.categoryPopUp = true;
  }

  close() {
    this.categoryPopUp = false;
  }

  save() {
    console.log("adding new category: " + this.categoryName);
    this.categoryAdded.emit(this.categoryName);
    this.close();
  }
}

import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-sort',
  templateUrl: './sort.component.html',
  styleUrls: ['./sort.component.scss']
})
export class SortComponent {

  @Output() sortSelected = new EventEmitter()
  sortOptions = [
    { name: 'Date: Oldest to Newest', value: 'dateAssc' },
    { name: 'Date: Newest to Oldest', value: 'dateDesc' },
    { name: 'Amount: Low to High', value: 'amountAsc' },
    { name: 'Amount: High to Low', value: 'amountDesc' },
  ];

  onSortSelected(event: any) {
    this.sortSelected.emit(event.target.value)
  }

}

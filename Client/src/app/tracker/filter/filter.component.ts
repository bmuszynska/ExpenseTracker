import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent {
  @Input() categories?: any[];
  @Input() trackerParams: any;
  @Output() categoryIdSelected = new EventEmitter<number>();
  @Output() monthIdSelected = new EventEmitter<number>();
 
  months: string[] = [];
  constructor(){
    this.months = this.generateMonthNames();
  }

  onCategorySelected(category: number) {
    this.categoryIdSelected.emit(category)
  }

  onMonthSelected(month: number) {
    this.monthIdSelected.emit(month)
  }

  private generateMonthNames(): string[] {
    const months: string[] = [];
    const date = new Date();

    for (let i = 0; i < 12; i++) {
      date.setMonth(i);
      months.push(date.toLocaleString('en-US', { month: 'long' }));
    }

    return ['Any', ...months];
  }

}

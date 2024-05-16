import { Component, OnInit } from '@angular/core';
import { Expense } from '../models/expense';
import { TrackerService } from './tracker.service';
import { Category } from '../models/category';
import { TrackerParams } from '../models/trackerParams';
@Component({
  selector: 'app-tracker',
  templateUrl: './tracker.component.html',
  styleUrls: ['./tracker.component.scss'],
})
export class TrackerComponent implements OnInit {
  expenses: Expense[] = [];
  categories: Category[] = [];
  months: string[] = [];
  trackerParams = new TrackerParams();
  sortOptions = [
    { name: 'Date: Oldest to Newest', value: 'dateAssc' },
    { name: 'Date: Newest to Oldest', value: 'dateDesc' },
    { name: 'Amount: Low to High', value: 'amountAsc' },
    { name: 'Amount: High to Low', value: 'amountDesc' },
  ];

  totalCount = 0;

  constructor(private trackerService: TrackerService) {
    this.months = this.generateMonthNames();
  }

  ngOnInit(): void {
    this.getExpenses();
    this.getCategories();
  }

  getExpenses() {
    this.trackerService.getExpenses(this.trackerParams).subscribe({
      next: (response) => {this.expenses = response.data
        this.trackerParams.pageNumber = response.pageIndex;
        this.trackerParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      }      ,
      error: (error) => console.log(error),
    });
  }

  getCategories() {
    this.trackerService.getCategories().subscribe({
      next: (response) =>
        (this.categories = [{ id: 0, name: 'All' }, ...response]),
      error: (error) => console.log(error),
    });
  }

  onCategorySelected(categoryId: number) {
    this.trackerParams.categoryId = categoryId;
    this.getExpenses();
  }

  onMonthSelected(monthId: number) {
    this.trackerParams.month = monthId;
    this.getExpenses();
  }

  onSortSelected(event: any) {
    this.trackerParams.sort = event.target.value;
    this.getExpenses();
  }

  onPageChanged(event: any) {
    if (this.trackerParams.pageNumber != event) {
      this.trackerParams.pageNumber = event;
      this.getExpenses();
    }
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

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
  trackerParams = new TrackerParams();
  totalCount = 0;

  constructor(private trackerService: TrackerService) {
  }

  ngOnInit(): void {
    this.getExpenses();
    this.getCategories();
  }

  onCategoryAdded(name: string) {
    this.trackerService.addCategory(name).subscribe({
      next: (response) =>
        console.log(response),
      error: (error) => console.log(error),
    });;
    this.ngOnInit();
  }

  getExpenses() {
    this.trackerService.getExpenses(this.trackerParams).subscribe({
      next: (response) => {
        this.expenses = response.data
        this.trackerParams.pageNumber = response.pageIndex;
        this.trackerParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
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

  onCategorySelected(event: any) {
    this.trackerParams.categoryId = event;
    this.getExpenses();
  }

  onMonthSelected(event: any) {
    this.trackerParams.month = event;
    this.getExpenses();
  }

  onSortSelected(event: any) {
    this.trackerParams.sort = event;
    this.getExpenses();
  }

  onPageChanged(page: number) {
    if (this.trackerParams.pageNumber != page) {
      this.trackerParams.pageNumber = page;
      this.getExpenses();
    }
  }


}

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TrackerComponent } from './tracker.component';
import { ExpenseItemModule } from './expense-item/expense-item.module';
import { SharedModule } from '../shared/shared.module';
import { HeaderComponent } from './header/header.component';
import { SortComponent } from './sort/sort.component';
import { ExpenseListComponent } from './expense-list/expense-list.component';
import { FilterComponent } from './filter/filter.component';
import { NewCategoryComponent } from './new-category/new-category.component';
import { NewExpenseComponent } from './new-expense/new-expense.component';

@NgModule({
  declarations: [TrackerComponent, HeaderComponent, SortComponent, ExpenseListComponent, FilterComponent, NewCategoryComponent, NewExpenseComponent],
  imports: [CommonModule, ExpenseItemModule, SharedModule],
  exports: [TrackerComponent],
})
export class TrackerModule {}

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TrackerComponent } from './tracker.component';
import { ExpenseItemModule } from './expense-item/expense-item.module';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [TrackerComponent],
  imports: [CommonModule, ExpenseItemModule, SharedModule],
  exports: [TrackerComponent],
})
export class TrackerModule {}

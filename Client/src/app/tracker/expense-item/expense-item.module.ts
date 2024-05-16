import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExpenseItemComponent } from './expense-item.component';



@NgModule({
  declarations: [ExpenseItemComponent],
  imports: [
    CommonModule
  ],
  exports:[ExpenseItemComponent]

})
export class ExpenseItemModule { }

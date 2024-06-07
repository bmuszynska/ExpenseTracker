import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-expense-list',
  templateUrl: './expense-list.component.html',
  styleUrls: ['./expense-list.component.scss']
})
export class ExpenseListComponent {
  @Input() expenses?: any[];
  @Output() expenseDeleted = new EventEmitter<number>();

  onExpenseDeleted(id: number) {
    this.expenseDeleted.emit(id);
  }
}

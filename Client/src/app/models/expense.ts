import { Category } from './category';

export interface Expense {
  category: Category;
  description: string;
  amount: number;
  date: string;
  id: number;
}

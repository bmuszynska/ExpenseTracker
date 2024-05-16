export interface Pagination<T> {
    count: number;
    data: T;
    pageIndex: number;
    pageSize: number;
  }
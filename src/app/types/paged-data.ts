// src/app/types/paged-data.ts
export interface PagedData<T> {
  data: T[];
  totalCount: number;
}

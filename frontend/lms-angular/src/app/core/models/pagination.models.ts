export interface PagedResult<T> { items: T[]; totalItems: number; totalPages: number; currentPage: number; pageSize: number; hasPreviousPage: boolean; hasNextPage: boolean; }
export interface CourseFilter { search?: string; isActive?: boolean; page: number; pageSize: number; }

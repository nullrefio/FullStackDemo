export interface PaginatedResponseModel {
    items: any[];
    order?: null | string;
    pageNumber: number;
    pageSize: number;
    search?: null | string;
    sortableFields: string[];
    totalItems: number;
    totalPages: number;
}

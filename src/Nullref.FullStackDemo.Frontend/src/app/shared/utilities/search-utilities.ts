import { Sort } from '@angular/material/sort';
import { PaginationEvent } from '../components/pagination-footer/models/pagination-event';

export abstract class SearchUtilities {

  // Search criteria can be empty or 3 or more chars
  static isValidSearchCriteria(text: string | null | undefined): any {
    const isValid = text ?? '';
    return (isValid.length >= 3 || isValid === '');
  }

  static getSort(text: string | null | undefined): Sort {
    const results = { active: '', direction: 'asc' } as Sort;
    if (text) {
      const arr = text.split(',');
      results.active = arr[0]?.toLowerCase();
      if (arr.length > 1) {
        if (arr[1]?.toLowerCase() === 'desc') {
          results.direction = 'desc';
        } else {
          results.direction = 'asc';
        }
      }
    }
    return results;
  }

  static toSortString(sort: Sort): string {
    if (!sort) { return ''; }
    if (sort.direction === '') { sort.direction = 'asc'; }
    return `${sort.active?.toLowerCase()},${sort.direction?.toLowerCase()}`;
  }

  static createDefaultResponseModel(): DefaultPaginatedResponseModel {
    return { pageNumber: 1, pageSize: 10, totalItems: 0, totalPages: 0, sortableFields: [], items: [], search: '', order: '' } as DefaultPaginatedResponseModel;
  }

  static createDefaultListItemsResponseModel(): DefaultListItemsResponseModel {
    return { items: [], order: '', search: '' } as DefaultListItemsResponseModel;
  }

  static handleEventPagination($event: PaginationEvent, dataSource: DefaultPaginatedResponseModel, reloadItemsAction: () => void | Promise<void>): void {
    if (dataSource.pageSize !== $event.pageSize) {
      $event.pageNumber = 1;
    }
    dataSource.pageNumber = $event.pageNumber;
    dataSource.pageSize = $event.pageSize;
    reloadItemsAction();
  }

  static handleEventSearch(text: string, dataSource: DefaultPaginatedResponseModel, reloadItemsAction: () => void | Promise<void>): void {
    dataSource.search = text;
    dataSource.pageNumber = 1;
    reloadItemsAction();
  }

  static handleEventSort($event: Sort, dataSource: DefaultPaginatedResponseModel, reloadItemsAction: () => void | Promise<void>): void {
    dataSource.order = SearchUtilities.toSortString($event);
    reloadItemsAction();
  }

  static findDuplicates(arry: string[]): string[] {
    const uniqueElements = new Set(arry);
    const result: string[] = [];
    uniqueElements.forEach(x => {
      if (arry.filter(z => z === x).length > 1) {
        result.push(x);
      }
    });
    return [...new Set(result)];
  }

}

export interface DefaultPaginatedResponseModel {
  items: any[];
  order?: string | null;
  pageNumber: number;
  pageSize: number;
  search?: string | null;
  sortableFields: string[];
  totalItems: number;
  totalPages: number;
}

export interface DefaultListItemsResponseModel {
  items: any[];
  order: string;
  search: string;
}

/* tslint:disable */
/* eslint-disable */
import { WidgetModel } from './widget-model';
export interface WidgetModelPaginatedResponseModel {
  items: Array<WidgetModel>;
  order?: null | string;
  pageNumber: number;
  pageSize: number;
  search?: null | string;
  sortableFields: Array<string>;
  totalItems: number;
  totalPages: number;
}

export const WidgetModelPaginatedResponseModelMetadata = {
  identifier: {
    items: 'items',
    order: 'order',
    pageNumber: 'pageNumber',
    pageSize: 'pageSize',
    search: 'search',
    sortableFields: 'sortableFields',
    totalItems: 'totalItems',
    totalPages: 'totalPages',
  },
  maxLength: {
    order: 100,
    search: 100,
  },
  minLength: {
  },
  readOnly: {
    items: false,
    order: false,
    pageNumber: false,
    pageSize: false,
    search: false,
    sortableFields: false,
    totalItems: false,
    totalPages: true,
  },
  required: {
    items: true,
    order: false,
    pageNumber: true,
    pageSize: true,
    search: false,
    sortableFields: true,
    totalItems: true,
    totalPages: true,
  },
  nullable: {
    items: false,
    order: true,
    pageNumber: false,
    pageSize: false,
    search: true,
    sortableFields: false,
    totalItems: false,
    totalPages: false,
  },
  default: {
  },
  format: {
    pageNumber: 'int32',
    pageSize: 'int32',
    totalItems: 'int32',
    totalPages: 'int32',
  },
  allowSort: {
  },
  displayName: {
  },
  description: {
  },
}

import { PaginatedResponseModel } from './paginated-response-model';

export interface ListPageData {
  allowAdd: boolean;
  allowEdit: boolean;
  allowDelete: boolean;
  isAdmin: boolean;
  allowView?: boolean;
  dataSource: PaginatedResponseModel;
}

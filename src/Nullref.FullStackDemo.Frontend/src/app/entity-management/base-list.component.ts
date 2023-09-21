import { Directive } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginationEvent } from '@app/shared/components/pagination-footer/models/pagination-event';
import { PaginatedResponseModel } from '@app/shared/models/paginated-response-model';
import { SearchUtilities } from '@app/shared/utilities/search-utilities';

@Directive({
  selector: '[appBaseListComponent]'
})
// eslint-disable-next-line @angular-eslint/directive-class-suffix
export abstract class BaseListComponent {

  allowAdd = true;
  allowEdit = true;
  allowDelete = true;
  allowView = true;

  dataSource: PaginatedResponseModel = SearchUtilities.createDefaultResponseModel();

  constructor(
    protected readonly route: ActivatedRoute,
    protected readonly router: Router,
  ) {
  }

  getSortDirection(): string {
    return SearchUtilities.getSort(this.dataSource.order).direction;
  }

  getSortActive(): string {
    return SearchUtilities.getSort(this.dataSource.order).active;
  }

  handleSearch($event: string): void {
    SearchUtilities.handleEventSearch($event, this.dataSource, () => this.reloadItems());
  }

  handlePagination($event: PaginationEvent): void {
    SearchUtilities.handleEventPagination($event, this.dataSource, () => this.reloadItems());
  }

  handleSort($event: Sort): void {
    SearchUtilities.handleEventSort($event, this.dataSource, () => this.reloadItems());
  }

  protected navigateToItemAdd(): void {
    this.router.navigate(['create'], { relativeTo: this.route });
  }

  protected navigateToItemEdit(id: string): void {
    this.router.navigate([id], { relativeTo: this.route });
  }

  protected reloadItems(): void {
  }
}

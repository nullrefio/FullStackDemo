import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { PaginationEvent } from './models/pagination-event';

@Component({
  selector: 'app-pagination-footer',
  templateUrl: './pagination-footer.component.html',
  styleUrls: ['./pagination-footer.component.scss']
})
export class PaginationFooterComponent implements OnInit, OnChanges {

  // NOTE: ALL PAGE NUMBERS SHOULD BE 1 BASED, NOT 0 BASED

  @Input() entityName = 'Items';
  @Input() currentPageNumber = 1;
  @Input() currentPageSize = 10;
  @Input() pageSizeOptions: number[] = [5, 10, 20];
  @Input() totalNumberOfPages = 10;
  @Input() totalNumberOfItems = 0;
  @Output() paginationEvent = new EventEmitter<PaginationEvent>();

  rangeOfVisiblePages: number[] = [];

  maxPagesShown = 5; // Maximum of page options to be shown in footer

  constructor() { }

  ngOnInit(): void {
    this.setRangeOfVisiblePageNumbers();
  }

  ngOnChanges(): void {
    this.setRangeOfVisiblePageNumbers();
  }

  setRangeOfVisiblePageNumbers(): void {
    this.rangeOfVisiblePages = [];
    for (let i = 2; i < this.totalNumberOfPages; i++) {
      if (i <= 5 && this.currentPageNumber <= 4) {
        this.rangeOfVisiblePages.push(i);
      }
      if (i >= 4 && i < this.totalNumberOfPages - 2 && this.currentPageNumber > 3 &&
        (i === this.currentPageNumber - 1 ||
          i === this.currentPageNumber ||
          i === this.currentPageNumber + 1)) {
        this.rangeOfVisiblePages.push(i);
      }

      if (i >= this.totalNumberOfPages - 4 && this.currentPageNumber >= this.totalNumberOfPages - 3) {
        this.rangeOfVisiblePages.push(i);
      }
    }
    this.rangeOfVisiblePages = [...new Set(this.rangeOfVisiblePages)];
  }

  previousPage(): void {
    this.currentPageNumber--;
    this.setRangeOfVisiblePageNumbers();
    this.emitPaginationEvent();
  }
  nextPage(): void {
    this.currentPageNumber++;
    this.setRangeOfVisiblePageNumbers();
    this.emitPaginationEvent();
  }
  pageChange(newPageNumber: number): void {
    this.currentPageNumber = newPageNumber;
    this.setRangeOfVisiblePageNumbers();
    this.emitPaginationEvent();
  }

  pageSizeChange(newPageSize: number): void {
    this.currentPageSize = newPageSize;
    this.setRangeOfVisiblePageNumbers();
    this.emitPaginationEvent();
  }

  private emitPaginationEvent(): void {
    this.paginationEvent.emit({
      pageNumber: this.currentPageNumber,
      pageSize: this.currentPageSize
    });
  }

}

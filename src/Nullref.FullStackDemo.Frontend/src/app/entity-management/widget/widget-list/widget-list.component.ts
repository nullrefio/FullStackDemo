import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { WidgetModelMetadata } from '@app/api/models/widget-model';
import { WidgetService } from '@app/api/services';
import { BaseListComponent } from '@app/entity-management/base-list.component';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-widget-list',
  templateUrl: './widget-list.component.html',
  styleUrls: ['./widget-list.component.scss']
})
export class WidgetListComponent extends BaseListComponent implements OnInit {
  metadata = WidgetModelMetadata;
  readonly displayedColumns = [this.metadata.identifier.code, this.metadata.identifier.myFruit, this.metadata.identifier.description, this.metadata.identifier.isActive];

  constructor(
    override readonly route: ActivatedRoute,
    override readonly router: Router,
    readonly widgetService: WidgetService
  ) {
    super(route, router);
  }

  async ngOnInit(): Promise<void> {
    await this.reloadItems();
  }

  async search(): Promise<void> {
    await this.reloadItems();
  }

  override async reloadItems(): Promise<void> {
    super.reloadItems();
    this.dataSource = await firstValueFrom(this.widgetService.apiV1WidgetsGet({
      PageNumber: this.dataSource.pageNumber,
      PageSize: this.dataSource.pageSize,
      Search: this.dataSource.search ?? '',
      Order: this.dataSource.order ?? '',
    }));
  }
}

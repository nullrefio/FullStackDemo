import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { WidgetModel } from '@app/api/models';
import { WidgetModelMetadata } from '@app/api/models/widget-model';
import { WidgetService } from '@app/api/services';
import { metaFormField } from '@app/shared/utilities/validation.utilities';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-widget-edit',
  templateUrl: './widget-edit.component.html',
  styleUrls: ['./widget-edit.component.scss']
})
export class WidgetEditComponent implements OnInit {
  readonly isEditMode: boolean;
  item?: WidgetModel;
  form: UntypedFormGroup;
  metadata = WidgetModelMetadata;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    readonly widgetService: WidgetService,
    formBuilder: UntypedFormBuilder
  ) {
    this.isEditMode = this.route.snapshot.data.mode === 'edit';
    this.form = formBuilder.group({
      [this.metadata.identifier.code]: metaFormField(this.metadata, this.metadata.identifier.code),
      [this.metadata.identifier.description]: metaFormField(this.metadata, this.metadata.identifier.description),
    });
  }

  async ngOnInit(): Promise<void> {
    const id = this.route.snapshot.params.id;
    this.item = await firstValueFrom(this.widgetService.apiV1WidgetsIdGet({ id: id }));
    this.form.patchValue(this.item);
  }

  onSubmit(): void {
    const body = this.form.value as WidgetModel;
    if (this.isEditMode) {
      this.widgetService.apiV1WidgetsIdPut({ id: this.item?.id!, body })
        .subscribe(() => {
          this.router.navigate(['..'], { relativeTo: this.route });
        });
    } else {
      this.widgetService.apiV1WidgetsPost({ body })
        .subscribe(() => {
          this.router.navigate(['..'], { relativeTo: this.route });
        });
    }
  }

  onDelete(): void {
    this.widgetService.apiV1WidgetsIdDelete({ id: this.item?.id! })
      .subscribe(() => {
        this.router.navigate(['..'], { relativeTo: this.route });
      });
  }
}
